import { useState } from "react";
import { useAuth } from "../Utils/AuthContext";
import { useNavigate } from "react-router-dom";

export default function LoginPage()
{
    const [error, setError] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();
    
    const { login, loading } = useAuth();

    async function handleSubmit(e)
    {
        e.preventDefault();

        setError("");

        try
        {
            await login(username, password);
            navigate("/dashboard");
        }
        catch(e)
        {
            setError(e.message);
        }
    }

    return (
        <div>
            <h2>Login</h2>

            {error && <p className="error">{error}</p>}

            <form onSubmit={handleSubmit}>
                <input
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />

                <input
                    placeholder="Password"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />

                <button disabled={loading} type="submit">
                    {loading ? "Logging in..." : "Login"}
                </button>
            </form>
        </div>
    );
}