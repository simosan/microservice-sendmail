package repository

import (
	"net/http"
	"os"

	"github.com/aws/aws-sdk-go/aws"
	"github.com/aws/aws-sdk-go/aws/credentials"
	"github.com/aws/aws-sdk-go/aws/session"
	"github.com/aws/aws-sdk-go/service/s3"
	"github.com/gin-gonic/gin"
)

func S3Upload(c *gin.Context, strarr []string) (int, error) {
	var rtn = 0

	// ファイルアップロード用のリクエストからファイルとフォームデータを取得
	file, _, err := c.Request.FormFile("upfile")
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "ファイル読み込みエラー"})
		rtn = http.StatusBadRequest
		return rtn, err
	}
	defer file.Close()

	// S3へのファイルアップロード
	sess, err := session.NewSession(&aws.Config{
		Region: aws.String(os.Getenv("AWSREGION")),
		Credentials: credentials.NewStaticCredentials(
			os.Getenv("AWSACCESSKEY"), os.Getenv("AWSSECRETKEY"), ""),
	})
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "AWSsession作成に失敗"})
		rtn = http.StatusInternalServerError
		return rtn, err
	}

	s3Client := s3.New(sess)

	// フォルダ名を含んだKeyを作成
	params := &s3.PutObjectInput{
		Bucket: aws.String(strarr[2]),
		Key:    aws.String(strarr[3]),
		Body:   file,
	}

	_, err = s3Client.PutObject(params)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "S3へのファイルアップロード失敗"})
		rtn = http.StatusInternalServerError
		return rtn, err
	}

	c.JSON(http.StatusOK, gin.H{"message": "S3へのファイルアップロード成功"})
	rtn = http.StatusOK

	return rtn, err
}
