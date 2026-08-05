import Auth from "./Auth.js";

export async function authFetch(url, options = {})
{
    let token = localStorage.getItem("token");

    let response = await fetch(url, {
        ...options,
        headers: {
            ...options.headers,
            Authorization: `Bearer ${token}`
        }
    });


    if (response.status === 401)
    {
        const refreshToken = localStorage.getItem("refreshToken");

        if (!refreshToken)
        {
            throw new Error("No refresh token");
        }


        const newTokens = await Auth.refreshUserAccess(refreshToken);


        localStorage.setItem(
            "token",
            newTokens.token
        );

        localStorage.setItem(
            "refreshToken",
            newTokens.refreshToken
        );


        response = await fetch(url, {
            ...options,
            headers: {
                ...options.headers,
                Authorization: `Bearer ${newTokens.token}`
            }
        });
    }


    return response;
}