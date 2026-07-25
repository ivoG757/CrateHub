import { createContext, useContext, useState, useEffect } from "react";
import Auth from "./api/Auth.js";

const AuthContext = createContext();

export function AuthProvider({ children }) 
{
    const [token, setToken] = useState(null);
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const [refreshToken, setRefreshToken] = useState(null);

    // Load token when app starts
    useEffect(() => 
    {
        const storedToken = localStorage.getItem("token");
        const storedRefreshToken = localStorage.getItem("refreshToken");

        if (storedToken && storedRefreshToken)
        {
            setToken(storedToken);
            setRefreshToken(storedRefreshToken);
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
            const tokens = await Auth.loginUser(username, password);

            localStorage.setItem("token", tokens.token); 
            localStorage.setItem("refreshToken", tokens.refreshToken)
            
            setToken(tokens.token);
            setRefreshToken(tokens.refreshToken)
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
            const tokens = await Auth.registerUser(email, username, password);

            localStorage.setItem("token", tokens.token); 
            localStorage.setItem("refreshToken", tokens.refreshToken)
            
            setToken(tokens.token);
            setRefreshToken(tokens.refreshToken)
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
        localStorage.removeItem("refreshToken")
        setToken(null);
        setRefreshToken(null);
        setUser(null);
    }

    async function refresh()
    {
        if (!refreshToken)
        {
            logout();
            throw new Error("Missing refresh token");
        }

        try
        {
            const tokens = await Auth.refreshUserAccess(refreshToken);

            localStorage.setItem("token", tokens.token);
            localStorage.setItem("refreshToken", tokens.refreshToken);

            setToken(tokens.token);
            setRefreshToken(tokens.refreshToken);

            return tokens.token;
        }
        
        catch(error)
        {
            logout();
            throw error;
        }
    }


    return (
        <AuthContext.Provider value=
        {{
            token,
            refreshToken,
            user,
            loading,
            refresh,
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