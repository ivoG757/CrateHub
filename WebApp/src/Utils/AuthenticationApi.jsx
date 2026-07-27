import { loadFiles, uploadFile, deleteFile } from "./api/Files.js"
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

    function deleteMyFile(id)
    {
        return deleteFile(getToken(), id)
    }
    
    return {
    loadMyFiles,
    deleteMyFile,
    uploadNewFile };
}