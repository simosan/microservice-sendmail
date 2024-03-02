package repository

import (
	"APPLICATION_PROCESSING/model"
	"errors"
	"log"
	"strconv"
	"time"

	"github.com/gin-gonic/gin"
	"gorm.io/gorm"
)

type ApplProcessRepository interface {
	CreateAppl(input *gin.Context) ([]string, error)
	CreateApplWithFile(input *gin.Context) ([]string, error)
	GetStaffAppl(input *gin.Context) ([]model.ApplHistories, error)
	GetSuperiorAppl(input *gin.Context) ([]model.ApplHistories, error)
	UpdateApproval(input *gin.Context) ([]model.MailTransInfo, error)
}

type applProcessRepository struct {
	db *gorm.DB
}

func NewApplProcess(db *gorm.DB) ApplProcessRepository {
	return &applProcessRepository{db}
}

func (a *applProcessRepository) CreateAppl(input *gin.Context) ([]string, error) {
	var strarr []string
	ctx := input.Request.Context()

	applTime, err := time.Parse("2006-01-02T15:04:05", input.PostForm("ApplTime"))
	if err != nil {
		log.Fatal(err)
	}

	var history = model.Histories{
		UserID:          input.PostForm("UserID"),
		Lastname:        input.PostForm("Lastname"),
		Firstname:       input.PostForm("Firstname"),
		Email:           input.PostForm("Email"),
		Position:        input.PostForm("Position"),
		Department:      input.PostForm("Department"),
		Section:         input.PostForm("Section"),
		MailDestination: input.PostForm("MailDestination"),
		MailSubject:     input.PostForm("MailSubject"),
		MailBody:        input.PostForm("MailBody"),
		ApplTime:        applTime,
		Supervisor_Id:   input.PostForm("Supervisor_Id"),
		Sp_Lastname:     input.PostForm("Sp_Lastname"),
		Sp_Firstname:    input.PostForm("Sp_Firstname"),
		Sp_Position:     input.PostForm("Sp_Position"),
		Sp_Department:   input.PostForm("Sp_Department"),
		Sp_Section:      input.PostForm("Sp_Section"),
		Approval_Time:   time.Date(1900, 1, 1, 0, 0, 0, 0, time.Local),
		ApplStatus:      0,
		ReasonForDenial: "",
		S3bucketName:    "",
		S3bucketKey:     "",
	}
	//入力された申請データをDBにIndert
	result := a.db.WithContext(ctx).Table("histories").Create(&history)
	if result.Error != nil {
		return strarr, result.Error
	}

	strarr = append(strarr, strconv.Itoa(int(history.Applno)), history.UserID)

	return strarr, nil
}

func (a *applProcessRepository) CreateApplWithFile(input *gin.Context) ([]string, error) {
	var strarr []string
	ctx := input.Request.Context()

	applTime, err := time.Parse("2006-01-02T15:04:05", input.PostForm("ApplTime"))
	if err != nil {
		log.Fatal(err)
	}

	var history = model.Histories{
		UserID:          input.PostForm("UserID"),
		Lastname:        input.PostForm("Lastname"),
		Firstname:       input.PostForm("Firstname"),
		Email:           input.PostForm("Email"),
		Position:        input.PostForm("Position"),
		Department:      input.PostForm("Department"),
		Section:         input.PostForm("Section"),
		MailDestination: input.PostForm("MailDestination"),
		MailSubject:     input.PostForm("MailSubject"),
		MailBody:        input.PostForm("MailBody"),
		ApplTime:        applTime,
		Supervisor_Id:   input.PostForm("Supervisor_Id"),
		Sp_Lastname:     input.PostForm("Sp_Lastname"),
		Sp_Firstname:    input.PostForm("Sp_Firstname"),
		Sp_Position:     input.PostForm("Sp_Position"),
		Sp_Department:   input.PostForm("Sp_Department"),
		Sp_Section:      input.PostForm("Sp_Section"),
		Approval_Time:   time.Date(1900, 1, 1, 0, 0, 0, 0, time.Local),
		ApplStatus:      0,
		ReasonForDenial: "",
		S3bucketName:    input.PostForm("S3bucketName"),
		S3bucketKey:     "",
	}
	//入力された申請データをDBにIndert
	result := a.db.WithContext(ctx).Table("histories").Create(&history)
	if result.Error != nil {
		return strarr, result.Error
	}

	strarr = append(strarr, strconv.Itoa(int(history.Applno)), history.UserID)

	// S3bucketKeyに申請番号とファイル名を付したパスを追加して更新
	pathkey := strarr[0] + "/" + input.PostForm("filename")
	updateFields := map[string]interface{}{
		"s3bucket_key": pathkey,
	}
	result2 := a.db.Table("histories").
		Where("applno=?", strarr[0]).
		Updates(updateFields)
	if result2.Error != nil {
		return strarr, result2.Error
	}
	//スライスstrarr（Applno、UserID）にs3bucketNameとs3bucket_patl（key）を追加
	strarr = append(strarr, input.PostForm("S3bucketName"))
	strarr = append(strarr, pathkey)

	return strarr, nil
}

func (a *applProcessRepository) GetStaffAppl(input *gin.Context) ([]model.ApplHistories, error) {
	var appl []model.ApplHistories
	ctx := input.Request.Context()

	// 自身の申請情報を取得
	// 否認ステータスかつ7日経過したものは取得対象外
	uid := input.Query("uid")
	result := a.db.WithContext(ctx).Table("histories").
		Select("applno", "user_id", "lastname", "firstname", "email",
			"position", "department", "section", "mail_destination",
			"mail_subject", "mail_body", "appl_time",
			"supervisor_id", "sp_lastname", "sp_firstname", "sp_position",
			"sp_department", "sp_section", "approval_time",
			"appl_status", "reason_for_denial", "s3bucket_name", "s3bucket_key").
		Where("user_id = ?", uid).
		Where("NOT (appl_status = ? AND (cast(now() as date) - cast(appl_time as date)) >= 7)", 1).
		Where("NOT (appl_status = ? AND (cast(now() as date) - cast(appl_time as date)) >= 7)", 2).
		Order("applno").
		Find(&appl)

	if result.Error != nil {
		return nil, result.Error
	}

	return appl, nil
}

func (a *applProcessRepository) GetSuperiorAppl(input *gin.Context) ([]model.ApplHistories, error) {
	var appl []model.ApplHistories
	ctx := input.Request.Context()

	uid := input.Query("uid")
	//appl_statusは、0（仕掛かり）を表示
	result := a.db.WithContext(ctx).Table("histories").
		Select("applno", "user_id", "lastname", "firstname", "email",
			"position", "department", "section", "mail_destination",
			"mail_subject", "mail_body", "appl_time",
			"supervisor_id", "sp_lastname", "sp_firstname", "sp_position",
			"sp_department", "sp_section", "approval_time",
			"appl_status", "reason_for_denial", "s3bucket_name", "s3bucket_key").
		Where("supervisor_id = ? AND appl_status = 0", uid).
		Order("applno").
		Find(&appl)

	if result.Error != nil {
		return nil, result.Error
	}
	return appl, nil
}

func (a *applProcessRepository) UpdateApproval(input *gin.Context) ([]model.MailTransInfo, error) {
	var denialmsg string
	var sendinfo []model.MailTransInfo
	ctx := input.Request.Context()

	apprvTime, err := time.Parse("2006-01-02T15:04:05", input.PostForm("Approval_Time"))
	if err != nil {
		log.Fatal(err)
	}

	//　承認:1 否認:2
	if input.PostForm("ApplStatus") == "1" {
		denialmsg = ""
	} else if input.PostForm("ApplStatus") == "2" {
		denialmsg = input.PostForm("ReasonForDenial")
	}

	updateFields := map[string]interface{}{
		"approval_time":     apprvTime,
		"appl_status":       input.PostForm("ApplStatus"),
		"reason_for_denial": denialmsg,
	}

	result := a.db.WithContext(ctx).Table("histories").
		Where("applno=?", input.PostForm("Applno")).
		Updates(updateFields)

	if result.Error != nil {
		return nil, result.Error
	}
	if result.RowsAffected == 0 {
		return nil, errors.New("更新対象のレコードがありませんでした")
	}
	applno, _ := strconv.Atoi(input.PostForm("Applno"))

	if input.PostForm("ApplStatus") == "1" {
		// メール送信情報を取得
		result := a.db.WithContext(ctx).Table("histories").
			Select("applno", "email", "mail_destination", "mail_subject",
				"mail_body", "s3bucket_name", "s3bucket_key").
			Where("applno = ?", applno).
			Find(&sendinfo)
		if result.Error != nil {
			return nil, result.Error
		}
	}

	return sendinfo, nil
}
