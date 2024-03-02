import React, { useState } from "react";
import { Modal, Button, Form } from 'react-bootstrap';
import { handleToTimestamp } from "../../commons/utils/Dateformat";

export const ApprovalForm = ({ showModal, handleCloseModal, handleNewApprovalSuccess, selectedApplno }) => {

    const [approvalStatus, setApprovalStatus] = useState("");
    const [reasonForDenial, setReasonForDenial] = useState("");

    const handleSubmit = async(e) => {
        e.preventDefault();
        const form = new FormData();
        //申請Noをセット
        form.set("Applno", selectedApplno);
        //承認時間を入力
        const apprvTime = handleToTimestamp(new Date());
        form.set("Approval_Time", apprvTime)

        if (approvalStatus === "approve") {
            // 承認ステータス（承認）
            form.set("ApplStatus", 1);
            form.set("ReasonForDenial", "")
        } else if (approvalStatus === "deny") {
            // 承認ステータス（否認）
            form.set("ApplStatus", 2);
            //　否認理由
            form.set("ReasonForDenial", reasonForDenial)
        }
        // フォームデータの送信
        try {
          const response = await fetch(`${process.env.REACT_APP_APPLICATIONPROCESS_URL}/approval`, {
            method: "POST",
            body: form,
          });
          const data = await response.json();
          console.log(data);
  
          // フォームデータの送信が完了したら成功処理を呼び出す
          handleNewApprovalSuccess();
        } catch (error) {
          console.error("Error submitting form:", error);
        }
        // モーダルを閉じる
        handleCloseModal();
    };

      return (
        <Modal show={showModal} onHide={handleCloseModal}>
          <Modal.Header closeButton>
            <Modal.Title>承認フォーム</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form>
              <Form.Group controlId="approvalStatus">
                <Form.Label>承認状態</Form.Label>
                <Form.Check
                  type="radio"
                  id="approveRadio"
                  label="承認"
                  checked={approvalStatus === "approve"}
                  onChange={() => setApprovalStatus("approve")}
                />
                <Form.Check
                  type="radio"
                  id="denyRadio"
                  label="否認"
                  checked={approvalStatus === "deny"}
                  onChange={() => setApprovalStatus("deny")}
                />
              </Form.Group>
    
              {approvalStatus === "deny" && (
                <Form.Group controlId="reasonForDenial">
                  <Form.Label>否認理由</Form.Label>
                  <Form.Control
                    as="textarea"
                    rows={3}
                    placeholder="否認理由を入力してください"
                    value={reasonForDenial}
                    onChange={(e) => setReasonForDenial(e.target.value)}
                  />
                </Form.Group>
              )}
            </Form>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="primary" onClick={handleSubmit}>
              Submit
            </Button>
            <Button variant="secondary" onClick={handleCloseModal}>
              キャンセル
            </Button>
          </Modal.Footer>
        </Modal>
      );
}

