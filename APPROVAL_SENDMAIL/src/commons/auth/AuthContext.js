import React, { createContext, useContext } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ user, children }) => (
  <AuthContext.Provider value={user}>{children}</AuthContext.Provider>
);

export const useAuth = () => {
  return useContext(AuthContext);
};
