import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import Swal from 'sweetalert2';

function Login({ setIsAuthenticated }) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [userType, setUserType] = useState('');
  const navigate = useNavigate();

  const handleLogin = e => {
    e.preventDefault();

    axios.post('https://localhost:7203/api/Auth/login', {
      username,
      password,
    })
    .then(response => {
      if (response.data.success) {
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('is_authenticated', true);
        setIsAuthenticated(true);
        if (userType === 'admin') {
          navigate('/admin');
        } else if (userType === 'teacher') {
          navigate('/teacher');
        }
        Swal.fire({
          icon: 'success',
          title: 'Successfully logged in!',
          showConfirmButton: false,
          timer: 1500,
        });
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Invalid credentials!',
          showConfirmButton: false,
          timer: 1500,
        });
      }
    });
  };
  
  const loginSuccess = () => {
    Swal.fire({
      timer: 1500,
      showConfirmButton: false,
      willOpen: () => {
        Swal.showLoading();
      },
      willClose: () => {
        localStorage.setItem('is_authenticated', true);
        setIsAuthenticated(true);

        Swal.fire({
          icon: 'success',
          title: 'Successfully logged in!',
          showConfirmButton: false,
          timer: 1500,
        });
      },
    });
  };

  return (
    <form onSubmit={handleLogin}>
      <label>
        Username:
        <input type="text" value={username} onChange={e => setUsername(e.target.value)} required />
      </label>
      <label>
        Password:
        <input type="password" value={password} onChange={e => setPassword(e.target.value)} required />
      </label>
      <label>
        User Type:
        <select value={userType} onChange={e => setUserType(e.target.value)} required>
          <option value="">Select user type</option>
          <option value="admin">Admin</option>
          <option value="teacher">Teacher</option>
        </select>
      </label>
      <button type="submit">Login</button>
    </form>
  );
};

export default Login;