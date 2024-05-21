import { Navigate, useLocation } from 'react-router-dom';

function ProtectedWrapper({ children, isAuthenticated }) {
  console.log('isAuthenticated in ProtectedWrapper:', isAuthenticated);
  const location = useLocation();

  return isAuthenticated ? children : <Navigate to="/login" state={{ from: location }} />;
}

export default ProtectedWrapper;