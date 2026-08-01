export async function uploadFile(token, file) {
    if (!file) {
        throw new Error("no file selected");
    }

    const formData = new FormData();
    formData.append("file", file);

    const response = await fetch("http://localhost:5127/api/files/upload", {
        method: "POST",
        headers: {
            Authorization: `Bearer ${token}`,
        },

        body: formData,
    });

    const data = await response.json();
    console.log(response);
    if (!response.ok) {
        throw new Error(data.message);
    }

    return data;
}
export async function downloadFile(shareToken) 
{
    window.location.href =
        `http://localhost:5127/api/files/download/${shareToken}`;

}
export async function fileInfo(shareToken) {
    const response = await fetch(
        `http://localhost:5127/api/files/share/${shareToken}`,
    );

    if (!response.ok) 
    {
        throw new Error("Failed to fetch file info");
    }

    const fileInfo = await response.json();
    console.log(fileInfo);
    return fileInfo;
}

export async function loadFiles(token) {
    const response = await fetch("http://localhost:5127/api/files", {
        method: "GET",
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
    const data = await response.json();
    console.log(response);
    if (!response.ok) {
        throw new Error(data.message);
    }

    return data;
}
export async function deleteFile(token, id) {
    const response = await fetch(`http://localhost:5127/api/files/${id}`, {
        method: "DELETE",
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
    console.log(response);
    if (!response.ok) {
        const data = await response.json();
        throw new Error(data.message);
    }

    return true;
}
