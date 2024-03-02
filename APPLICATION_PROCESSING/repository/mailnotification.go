package repository

import (
	"APPLICATION_PROCESSING/model"
	"bytes"
	"context"
	"encoding/json"
	"net/http"
	"os"

	"github.com/aws/aws-xray-sdk-go/xray"
)

func Mailnotification(ctx context.Context, sendinfo []model.MailTransInfo) (int, error) {

	notice := model.MailTransInfoJson{
		Email:           sendinfo[0].Email,
		MailDestination: sendinfo[0].MailDestination,
		MailSubject:     sendinfo[0].MailSubject,
		MailBody:        sendinfo[0].MailBody,
		S3bucketName:    sendinfo[0].S3bucketName,
		S3bucketKey:     sendinfo[0].S3bucketKey,
	}

	body, _ := json.Marshal(notice)

	client := xray.Client(http.DefaultClient)

	req, err := http.NewRequest(
		"POST",
		os.Getenv("SENDMAILURL"),
		bytes.NewBuffer(body))

	if err != nil {
		return 255, err
	}
	req.Header.Add("Content-Type", "application/json")

	resp, err := client.Do(req.WithContext(ctx))
	if err != nil {
		return 255, err
	}

	return resp.StatusCode, nil
}
