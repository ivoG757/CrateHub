import {useState} from 'react';
import { useAuth } from "../utils/AuthContext";
import { useNavigate } from "react-router-dom";
import "./RegisterPage.css";
import { PASSWORD, USERNAME, EMAIL } from "../constants/UserValidation.js";
export default function RegisterPage() 
{
    const [error, setError] = useState('');
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [password, setPassword] = useState('');
    const { loading, register } = useAuth();
    const navigate = useNavigate();

    async function handleSubmit(e)
    {
        e.preventDefault();
        setError("");


        if(!validateForm())
        {
            return;
        }

        try
        {
            await register(email, username, password);
            navigate("/dashboard")
        }

        catch(e)
        {
            setError(e.message);
        }
    }

    function validateForm()
    {
        if (!email || !username || !password || !confirmPassword)
        {
            setError("Please fill in all fields");
            return false;
        }

        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email))
        {
            setError("Please enter a valid email address");
            return false;
        }

        if (username.length < USERNAME.MIN_LENGTH || username.length > USERNAME.MAX_LENGTH)
        {
            setError(`Username must be between ${USERNAME.MIN_LENGTH} and ${USERNAME.MAX_LENGTH} characters`);
            return false;
        }

        if (password.length < PASSWORD.MIN_LENGTH || password.length > PASSWORD.MAX_LENGTH)
        {
            setError(`Password must be between ${PASSWORD.MIN_LENGTH} and ${PASSWORD.MAX_LENGTH} characters`);
            return false;
        }

        if (password !== confirmPassword)
        {
            setError("Passwords do not match");
            return false;
        }

        return true;
    }

    return (
        <div className="register-container">
    <div className="register-card">
        <h2>Create your account</h2>
        <p className="error">{error}</p>

        <form onSubmit={handleSubmit}>
            <div className="input-group">
                <label htmlFor="email">Email</label>
                <input
                    type="email"
                    id="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
            </div>

            <div className="input-group">
                <label htmlFor="username">Username</label>
                <input
                    type="text"
                    id="username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
            </div>

            <div className="input-group">
                <label htmlFor="password">Password</label>
                <input
                    type="password"
                    id="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
            </div>

            <div className="input-group">
                <label htmlFor="confirmPassword">Confirm Password</label>
                <input
                    type="password"
                    id="confirmPassword"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                />
            </div>

            <button type="submit">{loading ? "Loading" : "Register" }</button>
        </form>
    </div>
</div>
    );
}