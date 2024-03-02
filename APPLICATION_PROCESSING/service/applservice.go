package service

import (
	"APPLICATION_PROCESSING/model"
	"APPLICATION_PROCESSING/repository"

	"github.com/gin-gonic/gin"
)

type ApplProcessService interface {
	CreateAppl(input *gin.Context) ([]string, error)
	CreateApplWithFile(input *gin.Context) ([]string, error)
	GetStaffAppl(input *gin.Context) ([]model.ApplHistories, error)
	GetSuperiorAppl(input *gin.Context) ([]model.ApplHistories, error)
	UpdateApproval(input *gin.Context) ([]model.MailTransInfo, error)
}

type applProcessService struct {
	ar repository.ApplProcessRepository
}

func NewApplProcess(ur repository.ApplProcessRepository) ApplProcessService {
	return &applProcessService{ur}
}

func (as *applProcessService) CreateAppl(input *gin.Context) ([]string, error) {
	return as.ar.CreateAppl(input)
}

func (as *applProcessService) CreateApplWithFile(input *gin.Context) ([]string, error) {
	return as.ar.CreateApplWithFile(input)
}

func (as *applProcessService) GetStaffAppl(input *gin.Context) ([]model.ApplHistories, error) {
	return as.ar.GetStaffAppl(input)
}

func (as *applProcessService) GetSuperiorAppl(input *gin.Context) ([]model.ApplHistories, error) {
	return as.ar.GetSuperiorAppl(input)
}

func (as *applProcessService) UpdateApproval(input *gin.Context) ([]model.MailTransInfo, error) {
	return as.ar.UpdateApproval(input)
}
