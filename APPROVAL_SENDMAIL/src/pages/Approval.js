import React, { useEffect, useState } from "react";
import { Table } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useAuth } from "../commons/auth/AuthContext";
import { handleToDate } from "../commons/utils/Dateformat";
import { ApprovalForm } from "../components/forms/ApprovalForm";

const Approval = () => {
  const user = useAuth();
  const uidkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.userid";
  const titkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.title";
  let staff = false;
  let supervisor = false;
  let uid;

  if (user) {
    Object.entries(user).forEach(([key, value]) => {
      // userが存在するかどうかを確認
      if (key === titkey && value === "一般社員") {
        staff = true;
      } else if (key === titkey && value !== "一般社員") {
        supervisor = true;
      }
      if(key === uidkey) { uid = value }
    });
  }

  const [ approvallst, setApprovalllst] = useState([]);
  useEffect(() => {
    const fetchdata = async() => {
      try {
        const res = await fetch(
          `${process.env.REACT_APP_APPLICATIONPROCESS_URL}/supervisor?uid=${uid}`,
          {
            methoc: "GET",
            headers: {
              Accept: "application/json",
            },
          }
        );
        const data = await res.json();
        setApprovalllst(data)
      } catch (error) {
        console.error("Error fetchingdata:", error)
      }
    };
    fetchdata();
  }, [uid]);

  const [showModal, setShowModal] = useState(false);
  const [selectedApplno, setSelectedApplno] = useState(null);

  const handleOpenModal = (applno) => {
    setSelectedApplno(applno);
    setShowModal(true);
  }

  const handleCloseModal = () => {
    setSelectedApplno(null);
    setShowModal(false);
  };

  const handleNewApprovalSuccess = async () => {
    // 承認後にデータを再取得して画面を更新
    try {
      const res = await fetch(
        `${process.env.REACT_APP_APPLICATIONPROCESS_URL}/supervisor?uid=${uid}`,
        {
          method: "GET",
          headers: {
            Accept: "application/json",
          },
        }
      );
      const data = await res.json();
      setApprovalllst(data);
    } catch (error) {
      console.error("Error handleNewApplicationSuccess:fetchingdata:", error);
    }
  };

  return (
    <>
    {staff && (
      <h3>You are not authorized to access this content</h3> 
    )}
    {supervisor && ( 
      <div>
        <h1>承認</h1>
        <Table hover striped responsive>
          <thead>
            <tr>
            <th class="col-auto">ApplID</th>
            <th class="col-auto">ApplTime</th>
            <th class="col-auto">UserID</th>
            <th class="col-auto">Lastname</th>
            <th class="col-auto">Firstname</th>
            <th class="col-auto">Email</th>
            <th class="col-auto">Position</th>
            <th class="col-auto">Department</th>
            <th class="col-auto">Section</th>
            <th class="col-auto">MailDestination</th>
            <th class="col-auto">MailSubject</th>
            <th class="col-auto">MailBody</th>
            </tr>
          </thead>
          {approvallst.map((item) => (
            <tr key={item.Applno}>
              <td class="col-auto">
                <Link to="#" onClick={() => handleOpenModal(item.Applno)}>
                  {item.Applno}
                </Link>
              </td>
              <td class="col-auto ">{handleToDate(item.ApplTime)}</td>
              <td class="col-auto">{item.UserID}</td>
              <td class="col-auto">{item.Lastname}</td>
              <td class="col-auto">{item.Firstname}</td>
              <td class="col-auto">{item.Email}</td>
              <td class="col-auto">{item.Position}</td> 
              <td class="col-auto">{item.Department}</td> 
              <td class="col-auto">{item.Section}</td>
              <td class="col-auto">{item.MailDestination}</td>
              <td class="col-auto">{item.MailSubject}</td>
              <td class="col-auto">{item.MailBody}</td>
            </tr>
          ))}
        </Table>
        <ApprovalForm
          showModal={showModal}
          handleCloseModal={handleCloseModal}
          selectedApplno={selectedApplno}
          handleNewApprovalSuccess={handleNewApprovalSuccess}
        />
      </div>
    )}
    </>
  );
}
export default Approval;