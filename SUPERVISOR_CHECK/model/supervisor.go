package model

type Users struct {
	UserID       string `gorm:"column:user_id"`
	Lastname     string `gorm:"column:lastname"`
	Firstname    string `gorm:"column:firstname"`
	Email        string `gorm:"column:email"`
	PositionNo   int    `gorm:"column:position_no"`
	DepartmentNo int    `gorm:"column:department_no"`
}

type Departments struct {
	DepartmentNo int    `gorm:"column:department_no"`
	Department   string `gorm:"column:department"`
	Section      string `gorm:"column:section"`
}

type Positions struct {
	PositionNo   int    `gorm:"column:position_no"`
	PositionName string `gorm:"column:position_name"`
	RankValue    int    `gorm:"column:rankvalue"`
}

type UserSupervisors struct {
	UserID       string `gorm:"column:user_id"`
	Lastname     string `gorm:"column:lastname"`
	Firstname    string `gorm:"column:firstname"`
	Email        string `gorm:"column:email"`
	PositionNo   int    `gorm:"column:position_no"`
	DepartmentNo int    `gorm:"column:department_no"`
	Department   string `gorm:"column:department"`
	Section      string `gorm:"column:section"`
	PositionName string `gorm:"column:position_name"`
	RankValue    int    `gorm:"column:rankvalue"`
}
