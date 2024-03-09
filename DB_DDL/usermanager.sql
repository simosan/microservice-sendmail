--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2 (Homebrew)
-- Dumped by pg_dump version 16.2 (Homebrew)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: public; Type: SCHEMA; Schema: -; Owner: sim
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO sim;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: departments; Type: TABLE; Schema: public; Owner: sim
--

CREATE TABLE public.departments (
    department_no integer NOT NULL,
    department character varying(30) NOT NULL,
    section character varying(30) NOT NULL
);


ALTER TABLE public.departments OWNER TO sim;

--
-- Name: positions; Type: TABLE; Schema: public; Owner: sim
--

CREATE TABLE public.positions (
    position_no integer NOT NULL,
    position_name character varying(30) NOT NULL,
    rankvalue integer NOT NULL
);


ALTER TABLE public.positions OWNER TO sim;

--
-- Name: users; Type: TABLE; Schema: public; Owner: sim
--

CREATE TABLE public.users (
    user_id character varying(20) NOT NULL,
    lastname character varying(20) NOT NULL,
    firstname character varying(20) NOT NULL,
    email character varying(30) NOT NULL,
    position_no integer NOT NULL,
    department_no integer NOT NULL
);


ALTER TABLE public.users OWNER TO sim;

--
-- Name: departments departments_pkey; Type: CONSTRAINT; Schema: public; Owner: sim
--

ALTER TABLE ONLY public.departments
    ADD CONSTRAINT departments_pkey PRIMARY KEY (department_no);


--
-- Name: positions positions_pkey; Type: CONSTRAINT; Schema: public; Owner: sim
--

ALTER TABLE ONLY public.positions
    ADD CONSTRAINT positions_pkey PRIMARY KEY (position_no);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: sim
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: sim
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

