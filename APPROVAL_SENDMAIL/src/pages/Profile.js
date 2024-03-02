import React from "react";
import { useAuth } from "../commons/auth/AuthContext"
import { UserProfileAvatar } from "../components/user/UserProfileAvatar";

const Profile = () => {
  const user = useAuth();
  const depkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.department"
  const seckey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.section"
  const titkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.title"
  const uidkey = process.env.REACT_APP_AUTH0_CALLBACK_URL + "/user_metadata.userid"
  let department
  let section
  let title
  let uid

  Object.entries(user).forEach(([key, value]) => {
    if(key === depkey) { department = value } 
    if(key === seckey) { section = value }
    if(key === titkey) { title = value }
    if(key === uidkey) { uid = value}
  })

  return (
    <div>
      <h1>Profile</h1>
      <UserProfileAvatar style={{ width: "50px" }} />
      <h2>name</h2>
      <div>{user.name}</div>
      <h2>email</h2>
      <div>{user.email}</div>
      <h2>department</h2>
      <div>{department}</div>
      <h2>section</h2>
      <div>{section}</div>
      <h2>title</h2>
      <div>{title}</div>
      <h2>uid</h2>
      <div>{uid}</div>
    </div>
  );
};

export default Profile;