import { useAuth } from "../Utils/AuthContext";
import {useEffect, useState, useRef} from 'react';
import { useAuthenticatedApi } from "../Utils/AuthenticationApi";
import { useNavigate } from "react-router-dom";

export default function DashboardPage()
{
    const [error, setError] = useState("");
    const [uploading, setUploading] = useState(false);
    const [files, setFiles] = useState([]);
    const [uploadedFile, setUploadedFile] = useState();
    const { loading, user, logout } = useAuth();
    const fileInputRef = useRef();
    const navigate = useNavigate();
    const api = useAuthenticatedApi();

    
    async function upload()
    {
        
        setError("");
        setUploading(true)
        try 
        {
            const file = await api.uploadNewFile(uploadedFile);
            setFiles(current => [...current, file]);
        }

        catch(e)
        {
            setError(e.message);
        }

        finally
        {
            setUploading(false);
            setUploadedFile(null);
            fileInputRef.current.value = "";
        }
    }

   useEffect(() =>
    {
        async function load()
        {
            try
            {
                const filesFromDb = await api.loadMyFiles();
                setFiles(filesFromDb);
            }
            catch(e)
            {
                setError(e.message);
            }
        }

        if (user)
        {
            load();
        }

    }, [user]);

     if (loading)
     {
         return <p>Loading...</p>;
     }

     if (!user)
     {
         return null;
     }
 
    return(
    <div className="Dashboard-container">
        <h1>Welcome, {user.name}, with id: {user.id}</h1>
        <button onClick={logout}>logout</button>
        {files.length === 0 ? (<p>You haven't uploaded any files yet.</p>) : 
        (<ul>
            
            {files.map(file => 
            
            <li key={file.id}>
                <h3>{file.fileName}</h3> <p>Expires: {file.expiresAt}</p> <p>Uploaded at: {file.uploadedAt}</p>

                <button onClick={() => navigator.clipboard.writeText(file.downloadUrl)}>
                    Copy Link
                </button>
            </li>)}

        </ul>)}

        <input
            ref={fileInputRef}
            type="file"
            onChange={(e) => setUploadedFile(e.target.files[0])}
        />
        {uploadedFile && (<p>Selected: {uploadedFile.name}</p>)}
        
        {error && <p className="error">{error}</p>}
        <button onClick={upload} disabled={!uploadedFile || uploading}>{uploading ? "Uploading..." : "Submit"}</button>

    </div>)
}