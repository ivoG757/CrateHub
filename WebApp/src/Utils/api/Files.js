import {authFetch} from "./apiClient.js";
import {API_BASE_URL} from "../../config/constants.js";
export async function uploadNewFile(file) {
    if (!file) {
        throw new Error("no file selected");
    }

    const formData = new FormData();
    formData.append("file", file);

    const response = await authFetch(`${API_BASE_URL}/api/files/upload`, {
        method: "POST",
        body: formData,
    });

    const data = await tryParseResponse(response);
    
    console.log(response);

    if (!response.ok) 
    {
        throw new Error(data?.message || "Upload failed");
    }

    return data;
}
export async function downloadFile(shareToken) 
{
    window.location.href =
        `${API_BASE_URL}/api/files/download/${shareToken}`;

}
export async function fileInfo(shareToken) {
    const response = await authFetch(
        `${API_BASE_URL}/api/files/share/${shareToken}`, {}
    );
    const data = await tryParseResponse(response);

    if (!response.ok) 
    {
        throw new Error(data?.message || "Failed to fetch file info");
    }

    console.log(data);

    return data;
}

export async function loadMyFiles() {
    const response = await authFetch(`${API_BASE_URL}/api/files`, {
        method: "GET"
    });
    const data = await tryParseResponse(response);
    console.log(response);
    if (!response.ok) {
        throw new Error(data?.message || "Failed to load files");
    }

    return data;
}
export async function deleteMyFile(id) {
    const response = await authFetch(`${API_BASE_URL}/api/files/${id}`, {
        method: "DELETE"
    });
    console.log(response);
    if (!response.ok) {
        throw new Error("Failed to delete file");
    }

    return true;
}
async function tryParseResponse(response)
{
    try
    {
        return await response.json();
    }
    catch
    {
        return null;
    }
}
