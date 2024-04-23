import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";
import Avatar from "@mui/joy/Avatar";
import Button from "react-bootstrap/Button";
import { useContext } from "react";
import { NavLink } from "react-router-dom";

import LogRegisterContext from "../../contexts/log-register-context";

const NavigationBarBootstrap = (props) => {
  const logRegisterContext = useContext(LogRegisterContext);

  const logoutHandler = () => {
    logRegisterContext.setAdmin(logRegisterContext.token, false);
    logRegisterContext.logout();
  };
  return (
    <header>
      <Navbar bg="light" expand="lg" className="navigation-bar-bootstrap">
        <Container fluid style={{ margin: "0 10rem", padding: 0 }}>
          {(!logRegisterContext.isAdmin ||
            logRegisterContext.isAdmin === "false") && (
            <Navbar.Brand
              as={NavLink}
              to="/"
              style={{ color: "#b2b1b1", padding: 0 }}
            >
              {props.isTest ? (
                "Trip planner"
              ) : (
                <img
                  style={{ width: "210px", height: "30px", padding: 0 }}
                  src={require("../../assets/logo.png")}
                  alt="Trip planner"
                ></img>
              )}
            </Navbar.Brand>
          )}
          {logRegisterContext.isAdmin && (
            <Navbar.Brand
              as={NavLink}
              to="/"
              style={{ color: "#b2b1b1" }}
            ></Navbar.Brand>
          )}
          <Nav className="d-flex justify-content-end">
          {!logRegisterContext.token && (
            <a href="/api">|Click here for API Documentation|</a>
            )}
            {!logRegisterContext.token && (
              <Nav.Link
                style={{ margin: "0 0.5rem", color: "#b2b1b1" }}
                className="login-button-style"
                as={NavLink}
                to="/auth"
              >
                Login
              </Nav.Link>
            )}
            {logRegisterContext.token &&
              logRegisterContext.isAdmin !== "false" &&
              logRegisterContext.isAdmin && (
                <Nav.Link
                  style={{ margin: "0 0.5rem", color: "#b2b1b1" }}
                  className="login-button-style"
                  as={NavLink}
                  onClick={logoutHandler}
                >
                  Logout
                </Nav.Link>
              )}
            {logRegisterContext.token &&
              (!logRegisterContext.isAdmin ||
                logRegisterContext.isAdmin === "false") && (
                <Nav.Link
                  style={{ margin: "0 0.5rem", color: "#b2b1b1" }}
                  as={NavLink}
                  to="/trips"
                >
                  All trips
                </Nav.Link>
              )}
            {logRegisterContext.token &&
              (!logRegisterContext.isAdmin ||
                logRegisterContext.isAdmin === "false") && (
                <Nav.Link
                  style={{ margin: "0 0.5rem", color: "#b2b1b1" }}
                  as={NavLink}
                  to="/my-trips"
                  onClick={() => logRegisterContext.updateJoinedTrip(false)}
                >
                  My trips
                </Nav.Link>
              )}
            {/* {logRegisterContext.token && !logRegisterContext.isAdmin && (
              <Nav.Link
                style={{ margin: "0 0.5rem", color: "#b2b1b1" }}
                as={NavLink}
                to="/favorite-trips"
              >
                Favorite trips
              </Nav.Link>
            )} */}
            {logRegisterContext.token &&
              (!logRegisterContext.isAdmin ||
                logRegisterContext.isAdmin === "false") && (
                <Nav.Link
                  style={{ margin: "0 0.5rem", color: "#b2b1b1" }}
                  as={NavLink}
                  to="/new-trip"
                >
                  Create trip
                </Nav.Link>
              )}
            {logRegisterContext.token &&
              (!logRegisterContext.isAdmin ||
                logRegisterContext.isAdmin === "false") && (
                <Avatar
                  style={{ marginLeft: "0.5rem", color: "#b2b1b1" }}
                ></Avatar>
              )}
            {logRegisterContext.token &&
              (!logRegisterContext.isAdmin ||
                logRegisterContext.isAdmin === "false") && (
                <NavDropdown style={{zIndex: 10000}}>
                  {(!logRegisterContext.isAdmin ||
                    logRegisterContext.isAdmin === "false") && (
                    <NavDropdown.Item as={NavLink} to="/favorite-trips">
                      Favorite trips
                    </NavDropdown.Item>
                  )}
                  {(!logRegisterContext.isAdmin ||
                    logRegisterContext.isAdmin === "false") && (
                    <NavDropdown.Item as={NavLink} to="/leaderboard">
                      Leaderboards
                    </NavDropdown.Item>
                  )}
                  {(!logRegisterContext.isAdmin ||
                    logRegisterContext.isAdmin === "false") && (
                    <NavDropdown.Item as={NavLink} to="/profile">
                      Profile
                    </NavDropdown.Item>
                  )}
                  {(!logRegisterContext.isAdmin ||
                    logRegisterContext.isAdmin === "false") && (
                    <NavDropdown.Item as={NavLink} to="/help">
                      Help
                    </NavDropdown.Item>
                  )}
                  {(!logRegisterContext.isAdmin ||
                    logRegisterContext.isAdmin === "false") && (
                    <NavDropdown.Divider />
                  )}
                  {(!logRegisterContext.isAdmin ||
                    logRegisterContext.isAdmin === "false") && (
                    <NavDropdown.Item as={Button} onClick={logoutHandler}>
                      Logout
                    </NavDropdown.Item>
                  )}
                </NavDropdown>
              )}
          </Nav>
        </Container>
      </Navbar>
    </header>
  );
};

export default NavigationBarBootstrap;
