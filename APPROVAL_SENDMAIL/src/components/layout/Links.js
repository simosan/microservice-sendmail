import React from "react";
import { Nav } from "react-bootstrap";
import { useNavigate, useLocation } from "react-router-dom";
import { House, EmojiLaughing, SendPlus, Check2,TextCenter } from "react-bootstrap-icons";
import TextWithIcon from "../text/TextWithIcon";

const LinkItem = ({ pathname, pageName, icon }) => {
  const black = "black";
  const cyan = "#17A2AE";

  const navigate  = useNavigate();
  const location = useLocation();
  return (
    <Nav.Link
      onClick={() => {
        navigate(pathname);
      }}
      style={{ color: location.pathname === pathname ? cyan : black }}
    >
      <TextWithIcon icon={icon} text={pageName} />
    </Nav.Link>
  );
};

export const Links = () => {
  return (
    <Nav className="flex-column">
      <LinkItem
        pathname="/"
        pageName="Home"
        icon={<House />}
      />
      <LinkItem
        pathname="/appl"
        pageName="Application"
        icon={<SendPlus />}
      />
      <LinkItem
        pathname="/aprv"
        pageName="Approval"
        icon={<Check2 />}
      />
      <LinkItem
        pathname="/profile"
        pageName="Profile"
        icon={<EmojiLaughing />}
      />
    </Nav>
  );
};