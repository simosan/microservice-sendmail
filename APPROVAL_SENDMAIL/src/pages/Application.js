import React, { useState, useEffect } from "react";
import { Table, Button } from 'react-bootstrap';
import { useAuth } from "../commons/auth/AuthContext";
import { handleToDate } from "../commons/utils/Dateformat";
import ApplForm from "../components/forms/ApplForm";

const Application = () => {
  const user = useAuth();
  const uidkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.userid"
  let uid

  Object.entries(user).forEach(([key, value]) => {
    if(key === uidkey) {uid = value}
  })

  const statusMap = {
    "0": '申請中',
    '1': '承認',
    '2': '否認',
  };

  const [ appllst, setAppllst] = useState([]);

  useEffect(() => {
    const fetchdata = async() => {
      try{
        const res = await fetch(
          `${process.env.REACT_APP_APPLICATIONPROCESS_URL}/staff?uid=${uid}`,
          {
              method: "GET",
              headers: {
                  Accept: "application/json",
              },
          }
        );
        const data = await res.json();
        setAppllst(data)
      } catch (error) {
        console.error("Error fetchingdata:", error)
      }
    };
    fetchdata();
  },[uid]);

  const [showModal, setShowModal] = useState(false);
  
  const handleOpenModal = () => {
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  const handleNewApplication = () => {
    handleOpenModal();
  };

  const handleNewApplicationSuccess = async () => {
    // 申請成功時にデータを再取得して画面を更新
    try {
      const res = await fetch(
        `${process.env.REACT_APP_APPLICATIONPROCESS_URL}/staff?uid=${uid}`,
        {
          method: "GET",
          headers: {
            Accept: "application/json",
          },
        }
      );
      const data = await res.json();
      setAppllst(data);
    } catch (error) {
      console.error("Error handleNewApplicationSuccess:fetchingdata:", error);
    }
  };

  return(
    <div>
      <h1>メール申請</h1>
      <Button variant="primary" onClick={handleNewApplication}>
            新しい申請
      </Button>
      <ApplForm 
        showModal={showModal}
        handleCloseModal={handleCloseModal}
        handleNewApplicationSuccess={handleNewApplicationSuccess}  
      />

      <Table hover striped responsive>
        <thead>
          <tr>
            <th class="col-auto">ApplID</th>
            <th class="col-auto">ApplTime</th>
            <th class="col-auto">ApplStatus</th>
            <th class="col-auto">MailDestination</th>
            <th class="col-auto">MailSubject</th>
            <th class="col-auto">SpID</th>
            <th class="col-auto">SpLastname</th>
            <th class="col-auto">SpFirstname</th>
            <th class="col-auto">SpPosition</th>
            <th class="col-auto">SpDepartment</th>
            <th class="col-auto">ApprovalTime</th>
            <th class="col-auto">ReasonForDenial</th>
          </tr>
        </thead>
        <tbody>
          {appllst.map((item) => (
            <tr key={item.Applno}>
              <td class="col-auto">{item.Applno}</td>
              <td class="col-auto">{handleToDate(item.ApplTime)}</td>
              <td class="col-auto">{statusMap[item.ApplStatus]}</td>
              <td class="col-auto">{item.MailDestination}</td>
              <td class="col-auto">{item.MailSubject}</td>
              <td class="col-auto">{item.Supervisor_Id}</td>
              <td class="col-auto">{item.Sp_Lastname}</td>
              <td class="col-auto">{item.Sp_Firstname}</td>
              <td class="col-auto">{item.Sp_Position}</td>
              <td class="col-auto">{item.Sp_Department}</td>
              <td class="col-auto">{handleToDate(item.Approval_Time)}</td>
              <td class="col-auto">{item.ReasonForDenial}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default Application;