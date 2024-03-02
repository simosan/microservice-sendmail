// AWS SDK for JavaScript (v3) をインポートする
const { SESClient, SendRawEmailCommand } = require("@aws-sdk/client-ses");
const { S3Client, GetObjectCommand } = require("@aws-sdk/client-s3");
const fs = require("fs");
require('dotenv').config();

// SESClient を作成する
const sesClient = new SESClient({
    region: process.env.AWS_REGION,
    credentials: {
        accessKeyId: process.env.AWS_ACCESS_KEY_ID,
        secretAccessKey: process.env.AWS_SECRET_ACCESS_KEY 
    }
});

const postmail = async (req, res) => {
    const { MailDestination, MailSubject, 
            MailBody, Email,
            S3bucketName, S3bucketKey } = req.body;
    
    console.log(req.body)

    // S3から送信対象の添付ファイルを一時領域にダウンロード
    // S3 クライアントを作成する
    const s3Client = new S3Client({
        region: process.env.AWS_REGION,
        credentials: {
            accessKeyId: process.env.AWS_ACCESS_KEY_ID,
            secretAccessKey: process.env.AWS_SECRET_ACCESS_KEY 
        }
    });
    // GetObjectCommand を作成する
    const getObjectCommand = new GetObjectCommand({
        Bucket: S3bucketName,
        Key: S3bucketKey,
    });
    // S3 クライアントでコマンドを実行する
    const data = await s3Client.send(getObjectCommand);
    const pathfilename = process.env.WORKDIR + '/' + S3bucketKey
    const filename = S3bucketKey.split('/');
    fs.mkdir(process.env.WORKDIR + '/' + filename[0], {
        recursive: true 
    }, (err) => {
        if(err){
            throw err;
        }
    });

    const writer = fs.createWriteStream(pathfilename);
    writer.on("finish", () => {
        console.log("WriteSuccess:" + pathfilename)
        sendMail(filename[1]);
    });
    if (data.Body.pipe) {
        data.Body.pipe(writer);
    } else {
        // data.Body がストリームでない場合はそのまま書き込む
        writer.write(data.Body);
        writer.end();
    }

    async function fileStreamToBuffer(stream) {
        return new Promise((resolve, reject) => {
            const chunks = [];
            stream.on('data', (chunk) => chunks.push(chunk));
            stream.on('end', () => resolve(Buffer.concat(chunks)));
            stream.on('error', (error) => reject(error));
        });
    }

    //メール送信
    const sendMail = async () => {

        // ファイルを読み込んで Base64 エンコード
        const fileContent = fs.readFileSync(pathfilename, { encoding: "base64" });

        // メールの本文と添付ファイルを含むマルチパート MIME メッセージの作成
        const rawEmailData = `From: ${Email}
To: ${MailDestination}
Subject: ${MailSubject}
MIME-Version: 1.0
Content-Type: multipart/mixed; boundary="BOUNDARY"

--BOUNDARY
Content-Type: text/plain; charset="UTF-8"
Content-Transfer-Encoding: 7bit

${MailBody}

--BOUNDARY
Content-Type: application/octet-stream
Content-Disposition: attachment; filename="${filename[1]}"
Content-Transfer-Encoding: base64

${fileContent}
--BOUNDARY--
`;

        const sendEmailCommand = new SendRawEmailCommand({
            Source: Email,
            Destinations: [MailDestination],
            RawMessage: {
                Data: Buffer.from(rawEmailData),
            },
        });

        try {
            // AWS SES 経由でメールを送信
            const data = await sesClient.send(sendEmailCommand);
            console.log("Message sent: %s", data.MessageId);
            res.send("Email sent successfully");
        } catch (err) {
            console.error("Error sending email:", err);
            res.status(500).send("Error sending email");
        }
    };
}

module.exports = {
    postmail
}