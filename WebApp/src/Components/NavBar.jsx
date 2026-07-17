import { Link } from "react-router-dom";

export default function NavBar() 
{
    return (
        <nav>
            <h2 className="navBar-Title">FileShare</h2>

            <ul>
                <li>
                    <Link className="navBar-Link-Register" to="/login">
                        Login
                    </Link>
                </li>

                <li>
                    <Link className="navBar-Link-Login" to="/register">
                        Register
                    </Link>
                </li>
            </ul>
        </nav>
    );
}