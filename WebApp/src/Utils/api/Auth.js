async function registerUser(email, username, password)
{
    const response = await fetch("http://localhost:5127/api/authentication/register",
    {
        method: "POST",
        headers:
        {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, username, password })
    });

    const data = await handleResponse(response);

    return {
        token: data.accessToken,
        refreshToken: data.refreshToken
    };
}


async function loginUser(username, password)
{
    const response = await fetch("http://localhost:5127/api/authentication/login",
    {
        method: "POST",
        headers:
        {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ username, password })
    });

    const data = await handleResponse(response);

    return {
        token: data.accessToken,
        refreshToken: data.refreshToken
    };
}


async function getUser(token)
{
    const response = await fetch("http://localhost:5127/api/users/me",
    {
        method: "GET",
        headers:
        {
            "Authorization": `Bearer ${token}`
        }
    });

    return await handleResponse(response);
}


async function refreshUserAccess(refreshToken)
{
    const response = await fetch("http://localhost:5127/api/authentication/refresh",
    {
        method: "POST",
        headers:
        {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ token: refreshToken })
    });
    console.log("Fetching refresh")
    const data = await handleResponse(response);

    return {
        token: data.accessToken,
        refreshToken: data.refreshToken
    };
}


async function handleResponse(response)
{
    let data = null;

    try
    {
        data = await response.json();
    }
    catch
    {
        // empty response
    }


    if (!response.ok)
    {
        const error = new Error(data?.message ?? "Request failed");
        error.status = response.status;
        throw error;
    }


    return data;
}


export default {
    registerUser,
    refreshUserAccess,
    loginUser,
    getUser
};