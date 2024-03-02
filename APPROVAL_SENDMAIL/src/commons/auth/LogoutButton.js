import { useAuth0 } from "@auth0/auth0-react";
import React from "react";
import { Button } from "react-bootstrap";

const LogoutButton = (props) => {
  const { isAuthenticated, logout } = useAuth0();

  return isAuthenticated ? (
    <Button 
        variant="outline-primary"
        onClick={() => {
          logout({ returnTo: window.location.origin });
        }}
        {...props}
     >
      Log out
    </Button>
  ) : null;
}

export default LogoutButton;