import React from "react";
import LoginButton from "../../commons/auth/LoginButton";
import { Navbar } from "react-bootstrap";
import UserProfileDropdown from "../user/UserProfileDropdown";

export const Header = () => {
  
  return (
    <Navbar
      style={{
        borderBottom: "solid 1.5px #f3f4f4",
        height: "56px",
        marginBottom: "20px",
      }}
      bg="white"
      expand="lg"
      className="justify-content-between"
    >
      <Navbar.Brand href="/">{process.env.REACT_APP_NAME}</Navbar.Brand>
      <LoginButton />
      <UserProfileDropdown />
    </Navbar>
  );
};