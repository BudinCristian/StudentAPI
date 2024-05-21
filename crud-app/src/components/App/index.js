import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

import Login from '../Login';
import Admin from '../Admin/Admin';
import Teacher from '../Teacher/Teacher';
import ProtectedWrapper from '../Login/ProtectedWrapper';

const App = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const auth = JSON.parse(localStorage.getItem('is_authenticated'));
    setIsAuthenticated(auth);
    console.log('Auth from local storage:', auth);
    setIsLoading(false);
  }, []);

  if (isLoading) {
    return <div>Loading...</div>; // Or your custom loading component
  }

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login setIsAuthenticated={setIsAuthenticated} />} />
        <Route path="/admin" element={<ProtectedWrapper isAuthenticated={isAuthenticated}><Admin /></ProtectedWrapper>} />
        <Route path="/teacher" element={<ProtectedWrapper isAuthenticated={isAuthenticated}><Teacher /></ProtectedWrapper>} />
      </Routes>
    </Router>
  );
};
export default App;