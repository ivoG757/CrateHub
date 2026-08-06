import {Navigate} from "react-router-dom";
import {useAuth} from "../utils/AuthContext";

export default function LoggedInRoute({children}) 
{
    const {user} = useAuth();

    if(user)
    {
        return <Navigate to="/dashboard" replace />;
    }

    return children
}