import { createContext, useContext, useState, useEffect } from "react";
import Auth from "./api/auth/Auth.js";

const AuthContext = createContext();

export function AuthProvider({ children }) 
{
    const [token, setToken] = useState(null);
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    // Load token when app starts
    useEffect(() => 
    {
        const storedToken = localStorage.getItem("token");

        if (storedToken)
        {
            setToken(storedToken);
        }
        else
        {
            setLoading(false);
        }

    }, []);


    useEffect(() => 
    {
        async function fetchUser() 
        {
            if (!token)
            {
                setUser(null);
                setLoading(false);
                return;
            }

            try
            {
                const userData = await Auth.getUser(token);
                setUser(userData);
            }

            finally
            {
                setLoading(false);
            }
            
        }

        fetchUser();

    }, [token]);


    async function login(username, password)
    {
        setLoading(true);

        try
        {
            const newToken = await Auth.loginUser(username, password);

            localStorage.setItem("token", newToken);
            setToken(newToken);
        }

        catch(error)
        {
            logout();
            throw error;
        }

        finally
        {
            setLoading(false);
        }
    }

    async function register(email, username, password)
    {
        
        setLoading(true);

        try
        {
            const newToken = await Auth.registerUser(email, username, password);

            localStorage.setItem("token", newToken);
            setToken(newToken);
        }

        catch(error)
        {
            logout();
            throw error;
        }

        finally
        {
            setLoading(false);
        }
    }


    function logout() 
    {
        localStorage.removeItem("token");
        setToken(null);
        setUser(null);
    }


    return (
        <AuthContext.Provider value=
        {{
            token,
            user,
            loading,
            register,
            login,
            logout
        }}>
            {children}
        </AuthContext.Provider>
    );
}


export function useAuth()
{
    return useContext(AuthContext);
}