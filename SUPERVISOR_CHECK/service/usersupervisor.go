package service

import (
	"SUPERVISOR_CHECK/model"
	"SUPERVISOR_CHECK/repository"

	"github.com/gin-gonic/gin"
)

type UserSupervisorsService interface {
	GetSupervisors(c *gin.Context, Department string) ([]model.UserSupervisors, error)
}

type userSupervisorService struct {
	ur repository.UserSupervisorsRepository
}

func NewUserSupervisor(ur repository.UserSupervisorsRepository) UserSupervisorsService {
	return &userSupervisorService{ur}
}

func (us *userSupervisorService) GetSupervisors(c *gin.Context, Department string) ([]model.UserSupervisors, error) {
	return us.ur.GetSupervisors(c, Department)
}
