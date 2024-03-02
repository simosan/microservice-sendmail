package model

import "time"

type Histories struct {
	Applno          int       `gorm:"column:applno;autoIncrement;primaryKey"`
	UserID          string    `gorm:"column:user_id"`
	Lastname        string    `gorm:"column:lastname"`
	Firstname       string    `gorm:"column:firstname"`
	Email           string    `gorm:"column:email"`
	Position        string    `gorm:"column:position"`
	Department      string    `gorm:"column:department"`
	Section         string    `gorm:"column:section"`
	MailDestination string    `gorm:"column:mail_destination"`
	MailSubject     string    `gorm:"column:mail_subject"`
	MailBody        string    `gorm:"column:mail_body"`
	ApplTime        time.Time `gorm:"column:appl_time"`
	Supervisor_Id   string    `gorm:"column:supervisor_id"`
	Sp_Lastname     string    `gorm:"column:sp_lastname"`
	Sp_Firstname    string    `gorm:"column:sp_firstname"`
	Sp_Position     string    `gorm:"column:sp_position"`
	Sp_Department   string    `gorm:"column:sp_department"`
	Sp_Section      string    `gorm:"column:sp_section"`
	Approval_Time   time.Time `gorm:"column:approval_time"`
	ApplStatus      int       `gorm:"column:appl_status"`
	ReasonForDenial string    `gorm:"column:reason_for_denial"`
	S3bucketName    string    `gorm:"column:s3bucket_name"`
	S3bucketKey     string    `gorm:"column:s3bucket_key"`
}

type ApplHistories struct {
	Applno          int       `gorm:"column:applno"`
	UserID          string    `gorm:"column:user_id"`
	Lastname        string    `gorm:"column:lastname"`
	Firstname       string    `gorm:"column:firstname"`
	Email           string    `gorm:"column:email"`
	Position        string    `gorm:"column:position"`
	Department      string    `gorm:"column:department"`
	Section         string    `gorm:"column:section"`
	MailDestination string    `gorm:"mail_destination"`
	MailSubject     string    `gorm:"mail_subject"`
	MailBody        string    `gorm:"mail_body"`
	ApplTime        time.Time `gorm:"appl_time"`
	Supervisor_Id   string    `gorm:"supervisor_id"`
	Sp_Lastname     string    `gorm:"sp_lastname"`
	Sp_Firstname    string    `gorm:"sp_firstname"`
	Sp_Position     string    `gorm:"sp_position"`
	Sp_Department   string    `gorm:"sp_department"`
	Sp_Section      string    `gorm:"sp_section"`
	Approval_Time   time.Time `gorm:"approval_time"`
	ApplStatus      int       `gorm:"appl_status"`
	ReasonForDenial string    `gorm:"reason_for_denial"`
	S3bucketName    string    `gorm:"s3bucket_name"`
	S3bucketKey     string    `gorm:"s3bucket_key"`
}

type MailTransInfo struct {
	Applno          int    `gorm:"applno"`
	Email           string `gorm:"email"`
	MailDestination string `gorm:"mail_destination"`
	MailSubject     string `gorm:"mail_subject"`
	MailBody        string `gorm:"mail_body"`
	S3bucketName    string `gorm:"s3bucket_name"`
	S3bucketKey     string `gorm:"s3bucket_key"`
}

type MailTransInfoJson struct {
	Email           string `json:"Email"`
	MailDestination string `json:"MailDestination"`
	MailSubject     string `json:"MailSubject"`
	MailBody        string `json:"MailBody"`
	S3bucketName    string `json:"S3bucketName"`
	S3bucketKey     string `json:"S3bucketKey"`
}
