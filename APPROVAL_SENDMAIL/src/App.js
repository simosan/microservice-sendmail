import React from 'react';
import { useAuth0 } from "@auth0/auth0-react";
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { Layout } from "./components/layout/Layout";
import { AuthProvider } from "./commons/auth/AuthContext"
import ProtectedRoute from "./commons/auth/ProtectedRoute";
import Home from "./pages/Home";
import Profile from "./pages/Profile";
import Application from "./pages/Application";
import Approval from "./pages/Approval";

function App() {
  const { isLoading, user } = useAuth0();

  if (isLoading) {
    return <p>Loading...</p>;
  }

  return (
    <AuthProvider user={user}>
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<ProtectedRoute component={Home} />} />
          <Route path="appl" element={<ProtectedRoute component={Application} />} />      
          <Route path="aprv" element={<ProtectedRoute component={Approval} />} />
          <Route path="profile" element={<ProtectedRoute component={Profile} />} />
        </Routes>
      </Layout>
    </Router>
    </AuthProvider>
  );
}

export default App;
