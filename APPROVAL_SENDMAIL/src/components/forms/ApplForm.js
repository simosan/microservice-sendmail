import React, { useState, useEffect } from "react";
import { useAuth } from "../../commons/auth/AuthContext";
import { handleToTimestamp } from "../../commons/utils/Dateformat";
import { Modal, Button, Form } from 'react-bootstrap';

export const ApplForm = ({ showModal, handleCloseModal, handleNewApplicationSuccess }) => {
    const user = useAuth();
    const depkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.department"
    const seckey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.section"
    const titkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.title"
    const uidkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.userid"
    const lnkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.lastname"
    const fnkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.firstname"    
    let department
    let section
    let title
    let uid
    let lastname
    let firstname

    Object.entries(user).forEach(([key, value]) => {
        if(key === depkey) { department = value } 
        if(key === seckey) { section = value }
        if(key === titkey) { title = value }
        if(key === uidkey) { uid = value}
        if(key === lnkey) { lastname = value }
        if(key === fnkey) { firstname = value}
      })
    
    const [formData, setFormData] = useState({
        UserID: uid,
        Lastname: lastname,
        Firstname: firstname,
        Email: user.email,
        Position: title,
        Department: department,
        Section: section,
        MailDestination: '',
        MailSubject: '',
        MailBody: '',
        ApplTime: '',
        Supervisor_Id: '',
        Sp_Lastname: '',
        Sp_Firstname: '',
        Sp_Position: '',
        Sp_Department: department,
        Sp_Section: '',
        S3bucketName: 'sim-microservice-test',
        upfile: '',
        filename: ''
    });

    const [departmentOptions, setDepartmentOptions] = useState([]);
    useEffect(() => {
      if (formData.Department) {
        // APIサーバからデータを取得
        fetch(`${process.env.REACT_APP_SUPERVISOR_URL}?q=${formData.Department}`)
          .then(response => response.json())
          .then(data => {
            setDepartmentOptions(data);

            //データが取得できたら、最初の要素の上司情報をセット
            if (data.length > 0) {
              setFormData(prevFormData => ({
                ...prevFormData,
                Supervisor_Id: data[0].UserID,
                Sp_Lastname: data[0].Lastname,
                Sp_Firstname: data[0].Firstname,
                Sp_Position: data[0].PositionName,
                Sp_Department: data[0].Department,
                Sp_Section: data[0].Section
              }));
            }
          })
          .catch(error => {
            console.error("Error fetching supervisor data:", error);
          });
      };
    }, [formData.Department]);

    const handleFormChange = async (e) => {
      const { name, value } = e.target;
      setFormData({
          ...formData,
          [name]: value,
      });
      console.log("Updated formData:", formData);

      if (name === "Department") {
        // Departmentが変更されたときの処理
        try {
          const response = fetch(`${process.env.REACT_APP_SUPERVISOR_URL}?q=${value}`);
          const data = await response.json();
          setDepartmentOptions(data);
        }catch(error) {
            console.error("Error fetching supervisor data:", error);
        };
      }

      if (name === "Supervisor_Id") {
        // Supervisor_idが変更されたときの処理
        try {
          const response = await fetch(`${process.env.REACT_APP_SUPERVISOR_URL}?q=${formData.Department}`);
          const data = await response.json();
          // 選択された上司のIDに対応する姓を取得
          const selectedSupervisor = data.find(supervisor => supervisor.UserID === value);

          // データが取得できたら、Sp_Lastnameをセットする
          if (selectedSupervisor) {
            setFormData(prevFormData => ({
              ...prevFormData,
              Supervisor_Id: selectedSupervisor.UserID,
              Sp_Lastname: selectedSupervisor.Lastname,
              Sp_Firstname: selectedSupervisor.Firstname,
              Sp_Position: selectedSupervisor.PositionName,
              Sp_Department: selectedSupervisor.Department,
              Sp_Section: selectedSupervisor.Section
            }));
          }
        } catch (error) {
          console.error("Error fetching supervisor detail data:", error);
        }
      }
    };

    const handleFileChange = (e) => {
      const file = e.target.files[0];
      if(file ){
        //ファイルが選択された場合
        const filename = file.name;
        setFormData(prevFormData => ({
          ...prevFormData,
          upfile: file,
          filename: filename,
        }));
      }
    };

    const handleSubmitForm = async (e) => {
        e.preventDefault();

        const form = new FormData();
        // フォームデータを追加
        Object.entries(formData).forEach(([key, value]) => {
          form.append(key ,value);
        });
        //申請時間を入力
        const applTime = handleToTimestamp(new Date());
        form.set("ApplTime", applTime) 
        
        // ファイルが添付されていればファイルを追加
        if(formData.upfile) {
          form.append("upfile", formData.upfile);
        }

        //debut
        //console.log(formData);
        try {
        // フォームデータの送信
          const response = await fetch(`${process.env.REACT_APP_APPLICATIONPROCESS_URL}/appl`, {
            method: "POST",
            body: form,
          });
          const data = await response.json();
          console.log(data);
          handleNewApplicationSuccess();
        } catch (error) {
          console.error("Error submitting form: error");
        }
        
        handleCloseModal();
    };

    return (
        <Modal show={showModal} onHide={handleCloseModal}>
          <Modal.Header closeButton>
              <Modal.Title>申請フォーム</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            {/*<Form onSubmit={handleSubmitForm}>*/}
            <Form> 
              <Form.Group controlId="formUserID">
                {/* UserIDの選択肢 */}
                <Form.Label>ユーザID</Form.Label>
                <Form.Control
                   text="text"
                   as="textarea"
                   name="UserID"
                   value={formData.UserID}
                >
                </Form.Control>
              </Form.Group>
              <Form.Group controlId="formLastname">
              <Form.Label>氏名</Form.Label>
                <Form.Control
                  text="text"
                  as="textarea"
                  name="Lastname"
                  value={formData.Lastname}
                >
                </Form.Control>
              </Form.Group>
              <Form.Group controlId="formFirstname"> 
              <Form.Label>名前</Form.Label>
                <Form.Control
                  text="text"
                  as="textarea"
                  name="Firstname"
                  value={formData.Firstname}
                >
                </Form.Control>
              </Form.Group>                    
              <Form.Group controlId="formEmail">  
              <Form.Label>Email</Form.Label>
                <Form.Control
                  text="text"
                  as="textarea"
                  name="Email"
                  value={formData.Email}
                >
                </Form.Control>
              </Form.Group>                     
              <Form.Group controlId="formPosition">  
              <Form.Label>Position</Form.Label>
                <Form.Control
                  text="text"
                  as="textarea"
                  name="Position"
                  value={formData.Position}
                >
                </Form.Control>
              </Form.Group> 
              {/* Departmentの選択肢 */}
              <Form.Group controlId="formDepartment">
                <Form.Label>所属部署</Form.Label>
                <Form.Control
                   as="textarea"
                   name="Department"
                   value={formData.Department}
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>
              {/* Supervisor_idの選択肢 */}
              <Form.Group controlId="formSupervisor_Id">
                <Form.Label>上司ユーザID</Form.Label>
                <Form.Control
                  as="select"
                  name="Supervisor_Id"
                  onChange={(e) => handleFormChange(e)}
                >
                  {/* Sp_Departmentの選択肢をここに追加 */}
                  {departmentOptions.map(option => (
                    <option key={option.Department} value={option.UserID}>
                      {option.UserID}
                    </option>
                  ))}
                </Form.Control>
              </Form.Group>
              <Form.Group controlId="formSp_Lastname">
                <Form.Label>上司氏名</Form.Label>
                <Form.Control
                   as="textarea"
                   name="Sp_Lastname"
                   value={formData.Sp_Lastname}
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group> 
              <Form.Group controlId="formSp_Firstname">
                <Form.Label>上司名前</Form.Label>
                <Form.Control
                   as="textarea"
                   name="Sp_Firstname"
                   value={formData.Sp_Firstname}
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>
              <Form.Group controlId="formSp_Position">
                <Form.Label>上司役職</Form.Label>
                <Form.Control
                   as="textarea"
                   name="Sp_Position"
                   value={formData.Sp_Position}
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>
              <Form.Group controlId="formSp_Department">
                <Form.Label>上司部署</Form.Label>
                <Form.Control
                   as="textarea"
                   name="Sp_Department"
                   value={formData.Sp_Department}
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>
              <Form.Group controlId="formSp_Department">
                <Form.Label>上司課班</Form.Label>
                <Form.Control
                   as="textarea"
                   name="Sp_Section"
                   value={formData.Sp_Section}
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>
              <Form.Group controlId="formMailDestination">
                <Form.Label>メール宛先</Form.Label>
                <Form.Control
                   text="text"
                   as="textarea"
                   type="email"
                   placeholder="name@example.com"
                   name="MailDestination"
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>                    
              <Form.Group controlId="formMailSubject">
                <Form.Label>メール件名</Form.Label>
                <Form.Control
                   text="text"
                   as="textarea"
                   name="MailSubject"
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>                        
              <Form.Group controlId="formMailBody">
                <Form.Label>メール本文</Form.Label>
                <Form.Control
                   text="text"
                   as="textarea"
                   name="MailBody"
                   onChange={handleFormChange}
                >
                </Form.Control>
              </Form.Group>  
                <Form.Group controlId="formUpfile">
                <Form.Label>ファイル選択</Form.Label>
                <Form.Control
                    type="file"
                    name="upfile"
                    onChange={handleFileChange}
                />
              </Form.Group>
            </Form>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="primary" onClick={handleSubmitForm}>
              申請する
            </Button>
            <Button variant="secondary" onClick={handleCloseModal}>
              キャンセル
            </Button>
          </Modal.Footer>
        </Modal>
    );
};

export default ApplForm;