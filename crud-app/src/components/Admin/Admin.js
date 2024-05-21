import axios from 'axios';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Admin = () => {
  const [teachers, setTeachers] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const [newTeacher, setNewTeacher] = useState({ username: '', firstName: '', lastName: '', email: '', password: '' });

  axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    axios.get('https://localhost:7203/api/Teachers')
      .then(response => {
        setTeachers(response.data);
      })
      .catch(error => {
        console.error('There was an error!', error);
      });
  };

  const handleEdit = (id) => {
    setEditingId(id);
  };

  function handleInputChange(event, id) {
  const { name, value } = event.target;

  setTeachers(prevTeachers =>
    prevTeachers.map(teacher =>
      teacher.id === id ? { ...teacher, [name]: value } : teacher
    )
  );
}

  const handleUpdate = (id) => {
    const updatedTeacher = teachers.find(teacher => teacher.id === id);

    axios.put(`https://localhost:7203/api/Teachers/${id}`, updatedTeacher)
      .then(() => {
        setEditingId(null);
        fetchData();
      })
      .catch(error => {
        console.error('There was an error!', error);
      });
  };

  const handleDelete = (id) => {
    axios.delete(`https://localhost:7203/api/Teachers/${id}`)
      .then(() => {
        setTeachers(teachers.filter(teacher => teacher.id !== id));
      })
      .catch(error => {
        console.error('There was an error!', error);
      });
  };

  const handleNewTeacherChange = (event) => {
    const { name, value } = event.target;
    setNewTeacher({ ...newTeacher, [name]: value });
  };

  const handleCreate = (event) => {
    event.preventDefault();

    axios.post('https://localhost:7203/api/Teachers', newTeacher)
      .then(() => {
        setNewTeacher({ username: '', firstName: '', lastName: '', email: '', password: '' });
        fetchData();
      })
      .catch(error => {
        console.error('There was an error!', error);
      });
  };

  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.setItem('is_authenticated', false);
    navigate('/login');
  };

  return (
    <div>
      <h1>Teachers</h1>
      <div>
        <button onClick={handleLogout}>Logout</button>
      </div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Username</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Email</th>
            <th>Password</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {teachers.map((teacher) => (
            <tr key={teacher.id}>
              <td>{teacher.id}</td>
              <td>
                {editingId === teacher.id ? (
                  <input type="text" name="username" value={teacher.username} onChange={(event) => handleInputChange(event, teacher.id)} />
                ) : 
                  (teacher.username)
                }
              </td>
              <td>
                {editingId === teacher.id ? (
                    <input type="text" name="firstName" value={teacher.firstName} onChange={(event) => handleInputChange(event, teacher.id)} />
                  ) : (
                    teacher.firstName
                  )}
              </td>
              <td>
                {editingId === teacher.id ? (
                  <input type="text" name="lastName" value={teacher.lastName} onChange={(event) => handleInputChange(event, teacher.id)} />
                ) : (
                  teacher.lastName
                )}
              </td>
              <td>
                {editingId === teacher.id ? (
                    <input type="text" name="email" value={teacher.email} onChange={(event) => handleInputChange(event, teacher.id)} />
                  ) : (
                    teacher.email
                  )}
              </td>
              <td>
                {editingId === teacher.id ? (
                    <input type="text" name="password" value={teacher.password} onChange={(event) => handleInputChange(event, teacher.id)} />
                  ) : (
                    'Hidden'
                  )}
              </td>
              <td>
                {editingId === teacher.id ? (
                  <button onClick={() => handleUpdate(teacher.id)}>Save</button>
                ) : (
                  <button onClick={() => handleEdit(teacher.id)}>Edit</button>
                )
                },{
                <button onClick={() => handleDelete(teacher.id)}>Delete</button>
                }
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <div>
      <h2>Create New Teacher</h2>
      <form onSubmit={handleCreate}>
        <label>
          Username:
          <input type="text" name="username" value={newTeacher.username} onChange={handleNewTeacherChange} />
        </label>
        <label>
          First Name:
          <input type="text" name="firstName" value={newTeacher.firstName} onChange={handleNewTeacherChange} />
        </label>
        <label>
          Last Name:
          <input type="text" name="lastName" value={newTeacher.lastName} onChange={handleNewTeacherChange} />
        </label>
        <label>
          Email:
          <input type="text" name="email" value={newTeacher.email} onChange={handleNewTeacherChange} />
        </label>
        <label>
          Password:
          <input type="password" name="password" value={newTeacher.password} onChange={handleNewTeacherChange} />
        </label>
        <button type="submit">Create</button>
      </form>
    </div>
    </div>
  );
};

export default Admin;