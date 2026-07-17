import { useAuth } from "../Utils/AuthContext";
import {useEffect, useState, useRef} from 'react';
import {uploadFile, loadFiles} from '../Utils/api/Files';
import { useNavigate } from "react-router-dom";
export default function DashboardPage()
{
    const [error, setError] = useState("");
    const [uploading, setUploading] = useState(false);
    const { user, token, loading } = useAuth();
    const [files, setFiles] = useState([]);
    const [uploadedFile, setUploadedFile] = useState();
    const fileInputRef = useRef();
    const navigate = useNavigate();

    
    async function upload()
    {
        
        setError("");
        setUploading(true)
        try 
        {
            const file = await uploadFile(token, uploadedFile);
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
                const filesFromDb = await loadFiles(token);

                setFiles(filesFromDb)
            }
            catch(e)
            {
                setError(e.message);
            }
            
        }
        

        load();

    }, [token])

     if (loading)
        {
            return <p>Loading...</p>;
        }
    
        if (!user)
        {
            return navigate("/login")
        }

    return(
    <div className="Dashboard-container">
        <h1>Welcome, {user.name}, with id: {user.id}</h1>
        {files.length === 0 ? (<p>You haven't uploaded any files yet.</p>) : 
        (<ul>
            
            {files.map(file => 
            
            <li key={file.id}>
                <h3>{file.name}</h3> <p>Expires: {file.expiresAt}</p>
                <button>Copy Link</button>
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