import { useEffect, useRef, useState } from "react";
import { useAuth } from "../utils/AuthContext.jsx";
import { loadMyFiles, uploadNewFile, deleteMyFile } from "../utils/api/Files.js"
import { MAX_FILE_SIZE } from "../constants/FileValidation.js";

export default function DashboardPage() {
    const { user, logout } = useAuth();

    const [files, setFiles] = useState([]);
    const [uploadedFile, setUploadedFile] = useState(null);
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState("");

    const fileInputRef = useRef(null);

    useEffect(() => 
    {
        async function loadFiles() {
            try 
            {
                const result = await loadMyFiles();
                setFiles(result);
            }
            catch (err) 
            {
                setError(err.message);
            }
        }

        loadFiles();
    }, []);

    function handleFileUpload(event) {
        const file = event.target.files[0];
        setError("");

        if (!file) return;

        if (file.size > MAX_FILE_SIZE) {
            setError("File is too large. Maximum size is 1 GB.");
            setUploadedFile(null);
            event.target.value = "";
            return;
        }

        setUploadedFile(file);
    }

    async function removeFile(id)
    {
        console.log(`deleting: ${id}`);

        try
        {
            await deleteMyFile(id);

            setFiles(current =>
                current.filter(file => file.id !== id)
            );
        }
        catch(e)
        {
            setError(e.message);
        }
    }


    async function upload() {
        if (!uploadedFile)
            return;

        setUploading(true);
        setError("");

        try {
            const newFile = await uploadNewFile(uploadedFile);

            setFiles(current => [...current, newFile]);

            setUploadedFile(null);

            if (fileInputRef.current)
                fileInputRef.current.value = "";
        }
        catch (err) 
        {
            setError(err.message);
        }
        finally 
        {
            setUploading(false);
        }
    }

    function getShareUrl(token)
    {
        return `${window.location.origin}/shared/${token}`;
    }

    async function copyLink(url) {
        try {
            await navigator.clipboard.writeText(url);
            alert("Copied!");
        }
        catch {
            alert("Failed to copy.");
        }
    }

    return (
        <div className="Dashboard-container">

            <h1>Welcome, {user.name}</h1>

            <button onClick={logout}>
                Logout
            </button>

            <hr />

            {files.length === 0 ? (
                <p>No files uploaded yet.</p>
            ) : (
                <ul>
                    {files.map(file => (
                        <li key={file.id}>

                            <h3>{file.fileName}</h3>

                            <p>Uploaded: {file.uploadedAt}</p>

                            <p>Expires: {file.expiresAt}</p>

                            <button
                                onClick={() => copyLink(getShareUrl(file.shareToken))}
                            >
                                Copy Link
                            </button>

                            <button
                                onClick={() => removeFile(file.id)}
                                >
                                Delete
                            </button>

                        </li>
                    ))}
                </ul>
            )}

            <hr />

            <input
                ref={fileInputRef}
                type="file"
                onChange={e => handleFileUpload(e)}
            />

            {uploadedFile && (
                <p>Selected: {uploadedFile.name}</p>
            )}

            {error && (
                <p className="error">{error}</p>
            )}

            <button
                disabled={!uploadedFile || uploading}
                onClick={upload}
            >
                {uploading ? "Uploading..." : "Upload"}
            </button>

        </div>
    );
}