import { createContext, useContext, useEffect, useState } from "react";
import Auth from "./api/Auth.js";


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
            const refreshToken = localStorage.getItem("refreshToken");


            if (!token || !refreshToken)
            {
                setLoading(false);
                return;
            }


            try
            {
                const currentUser = await api.getUser(token);

                setUser(currentUser);
            }
            catch(error)
            {
                if(error.status !== 401)
                {
                    console.error(error);
                    logout();
                    setLoading(false);
                    return;
                }


                try
                {
                    const newTokens = await api.refreshUserAccess(refreshToken);


                    localStorage.setItem(
                        "token",
                        newTokens.token
                    );

                    localStorage.setItem(
                        "refreshToken",
                        newTokens.refreshToken
                    );


                    const currentUser = await api.getUser(newTokens.token);

                    setUser(currentUser);
                }
                catch
                {
                    logout();
                }
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



    async function register(email, username, password)
    {
        const tokens = await api.registerUser(
            email,
            username,
            password
        );


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



    function getToken()
    {
        return localStorage.getItem("token");
    }



    return (
        <AuthContext.Provider
            value={{
                user,
                loading,
                login,
                register,
                logout,
                getToken
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