import { createContext, useContext, useEffect, useState } from "react";
import Auth  from "./api/Auth.js";

const AuthContext = createContext(null);

export function AuthProvider({ children })
{
    const api = Auth;

    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() =>
    {
        async function load()
        {
            const token = localStorage.getItem("token");

            if (!token)
            {
                setLoading(false);
                return;
            }

            try
            {
                const currentUser = await api.getUser(token);
                setUser(currentUser);
            }
            catch
            {
                logout();
            }
            finally
            {
                setLoading(false);
            }
        }

        load();
    }, []);

    async function login(username, password)
    {
        const tokens = await api.loginUser(username, password);

        localStorage.setItem("token", tokens.token);
        localStorage.setItem("refreshToken", tokens.refreshToken);

        const currentUser = await api.getUser(tokens.token);

        setUser(currentUser);
    }

    function getToken()
    {
        return localStorage.getItem("token");
    }

    async function register(email, username, password)
    {
        const tokens = await api.registerUser(email, username, password);

        localStorage.setItem("token", tokens.token);
        localStorage.setItem("refreshToken", tokens.refreshToken);

        const currentUser = await api.getUser(tokens.token);

        setUser(currentUser);
    }

    function logout()
    {
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        setUser(null);
    }

    return (
        <AuthContext.Provider
            value={{
                user,
                loading,
                login,
                getToken,
                register,
                logout
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth()
{
    return useContext(AuthContext);
}