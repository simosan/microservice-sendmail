const express = require('express');
const app = express();
const smail = require('./sendmail');
require('dotenv').config();

const cors = require('cors');
const bdparser = require('body-parser');
const morgan = require('morgan');
const helmet = require('helmet');
const awsxray = require('aws-xray-sdk')

const white = ['WHITELISTURL'];
const corsopt = {
    origin: function (origin, callback) {
        if (white.indexOf(origin) !== -1 || !origin) {
            callback(null, true);
        } else {
            callback(new Error('許可されていないCORSです'));
        }
    }
}

app.use(helmet());
app.use(cors(corsopt));
app.use(bdparser.json());
app.use(morgan('combined'));

app.use(awsxray.express.openSegment('SESMailClient'));
app.get('/', (req, res) => res.send('サーバー実行中!'));
app.post('/sendmail', (req, res) => smail.postmail(req, res));
app.use(awsxray.express.closeSegment());

app.listen(process.env.PORT || 33000, () => {
    console.log(`port ${process.env.PORT || 33000}`);
})