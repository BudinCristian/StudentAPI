import axios from 'axios';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Teacher = () => {
  const [students, setStudents] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const [newStudent, setNewStudent] = useState({ firstName: '', lastName: '', email: '', age: '', groupId: '' });

  axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    axios.get('https://localhost:7203/api/Students')
      .then(response => {
        setStudents(response.data);
      })
      .catch(error => {
        console.error('There was an error!', error);
      });
  };

  const handleEdit = (id) => {
    setEditingId(id);
  };

  const handleInputChange = (event, id) => {
    const { name, value } = event.target;
    setStudents(students.map(student => student.id === id ? { ...student, [name]: value } : student));
  };
  
  const handleUpdate = (id) => {
    const updateStudent = students.find(student => student.id === id);

    axios.put(`https://localhost:7203/api/Students/${id}`, updateStudent)
      .then(() => {
        setEditingId(null);
        fetchData();
      })
      .catch(error => {
        console.error('There was an error!', error);
      });
  };

  const handleDelete = (id) => {
    axios.delete(`https://localhost:7203/api/Students/${id}`)
      .then(() => {
        setStudents(students.filter(student => student.id !== id));
      })
      .catch(error => {
        console.error('There was an error!', error);
      });
  };

  const handleNewStudentChange = (event) => {
    const { name, value } = event.target;
    setNewStudent({ ...newStudent, [name]: value });
  };

  const handleCreate = (event) => {
    event.preventDefault();

    axios.post('https://localhost:7203/api/Students', newStudent)
      .then(() => {
        setNewStudent({firstName: '', lastName: '', email: '', age: '', groupId: ''});
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
      <h1>Students</h1>
      <div>
        <button onClick={handleLogout}>Logout</button>
      </div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Email</th>
            <th>Age</th>
            <th>Group ID</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
        {students.map((student) => (
          <tr key={student.id}>
            <td>{student.id}</td>
            <td>
              {editingId === student.id ? (
                <input type="text" name="firstName" value={student.firstName} onChange={(event) => handleInputChange(event, student.id)} />
              ) : (
                student.firstName
              )}
            </td>
            <td>
              {editingId === student.id ? (
                <input type="text" name="lastName" value={student.lastName} onChange={(event) => handleInputChange(event, student.id)} />
              ) : (
                student.lastName
              )}
            </td>
            <td>
              {editingId === student.id ? (
                <input type="text" name="email" value={student.email} onChange={(event) => handleInputChange(event, student.id)} />
              ) : (
                student.email
              )}
            </td>
            <td>
              {editingId === student.id ? (
                <input type="text" name="age" value={student.age} onChange={(event) => handleInputChange(event, student.id)} />
              ) : (
                student.age
              )}
            </td>
            <td>
              {editingId === student.id ? (
                <input type="text" name="groupId" value={student.groupId} onChange={(event) => handleInputChange(event, student.id)} />
              ) : (
                student.groupId
              )}
            </td>
            <td>
              {editingId === student.id ? (
                <button onClick={() => handleUpdate(student.id)}>Save</button>
              ) : (
                <button onClick={() => handleEdit(student.id)}>Edit</button>
              )},{
                <button onClick={() => handleDelete(student.id)}>Delete</button>
                }
            </td>
          </tr>
        ))}
        </tbody>
      </table>
      <div>
      <h2>Create New Student</h2>
      <form onSubmit={handleCreate}>
        <label>
          First Name:
          <input type="text" name="firstName" value={newStudent.firstName} onChange={handleNewStudentChange} />
        </label>
        <label>
          Last Name:
          <input type="text" name="lastName" value={newStudent.lastName} onChange={handleNewStudentChange} />
        </label>
        <label>
          Email:
          <input type="text" name="email" value={newStudent.email} onChange={handleNewStudentChange} />
        </label>
        <label>
          Age:
          <input type="number" name="age" value={newStudent.age} onChange={handleNewStudentChange} />
        </label>
        <label>
          Group ID:
          <input type="number" name="groupId" value={newStudent.groupId} onChange={handleNewStudentChange} />
        </label>
        <button type="submit">Create</button>
      </form>
      </div>

    </div>
  );
};

export default Teacher;