import { loadFiles, uploadFile } from "./api/Files";
import { useAuth } from "./AuthContext";
export function useAuthenticatedApi() 
{
    const { token, refresh, login } = useAuth();

    async function loadMyFiles() 
    {
        try 
        {
            return await loadFiles(token);
        }

        catch (e)
        {
            if (e.status === 401)
            {
                const newToken = await refresh();
                return await loadFiles(newToken);
            }

            throw e;
        }
    }

    async function uploadNewFile(file)
    {
        try 
        {
            return await uploadFile(token, file);
        }

        catch (e)
        {
            if (e.status === 401)
            {
                const newToken = await refresh();
                return await uploadFile(newToken, file);
            }

            throw e;
        }
    }
    
    return {
        loadMyFiles,
        uploadNewFile
    };
}