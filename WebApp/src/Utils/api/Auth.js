async function registerUser(email, username, password) 
{
    const response = await fetch('http://localhost:5127/api/authentication/register', 
    {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, username, password })
    });

    const data = await response.json();
    console.log(data);
    console.log(response) //debug

    if (!response.ok)
    {
        throw new Error(data.message);
    }
    
     return {token: data.accessToken, refreshToken: data.refreshToken};
}

async function loginUser(username, password)
{
    const response = await fetch('http://localhost:5127/api/authentication/login', 
    {
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json' 
        },
        body: JSON.stringify({ username, password })
    });
    
    console.log(response) //debug
    const data = await response.json();
    
    console.log("BODY:", data);

    if (!response.ok)
    {
        throw new Error(data.message);
    }


     return {token: data.accessToken, refreshToken: data.refreshToken};
}

async function getUser(token)
{
    const response = await fetch('http://localhost:5127/api/users/me',
    {
        method: "GET",
        headers:
        {
            "Authorization": `Bearer ${token}`
        }
    });

    const data = await response.json();
    
    console.log(response) //debug

    if (!response.ok)
    {
        throw new Error(data.message);
    }

    return data;
}

async function refreshUserAccess(refreshToken)
{
    const response = await fetch('http://localhost:5127/api/authentication/refresh',
    {
        method: 'POST',
        headers:
        {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ refreshToken })
    });

    if (!response.ok)
    {
        throw new Error(data.message);
    }
    
    const data = await response.json();

     return {token: data.accessToken, refreshToken: data.refreshToken};
}
export default { registerUser, refreshUserAccess, loginUser, getUser };