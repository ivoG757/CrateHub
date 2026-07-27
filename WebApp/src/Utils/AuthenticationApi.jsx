import {loadFiles, uploadFile } from "./api/Files.js"
import { useAuth } from "./AuthContext.jsx"

export default function useAuthenticatedApi()
{
    const { getToken} = useAuth();

    function loadMyFiles()
    {
        return loadFiles(getToken());
    }

    function uploadNewFile(file)
    {
        return uploadFile(getToken(), file);
    }
    
    return {
    loadMyFiles,
    uploadNewFile };
}