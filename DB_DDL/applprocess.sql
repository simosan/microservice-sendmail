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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: histories; Type: TABLE; Schema: public; Owner: sim
--

CREATE TABLE public.histories (
    applno integer NOT NULL,
    user_id character varying(20) NOT NULL,
    lastname character varying(20) NOT NULL,
    firstname character varying(20) NOT NULL,
    email character varying(30) NOT NULL,
    "position" character varying(30) NOT NULL,
    department character varying(30) NOT NULL,
    section character varying(30) NOT NULL,
    mail_destination character varying(30) NOT NULL,
    mail_subject character varying(100) NOT NULL,
    mail_body character varying(9999) NOT NULL,
    appl_time timestamp with time zone NOT NULL,
    supervisor_id character varying(20) NOT NULL,
    sp_lastname character varying(20) NOT NULL,
    sp_firstname character varying(20) NOT NULL,
    sp_position character varying(30) NOT NULL,
    sp_department character varying(30) NOT NULL,
    sp_section character varying(30) NOT NULL,
    approval_time timestamp with time zone,
    appl_status integer NOT NULL,
    reason_for_denial character varying(9999),
    s3bucket_name character varying(60),
    s3bucket_key character varying(1024)
);


ALTER TABLE public.histories OWNER TO sim;

--
-- Name: histories_applno_seq; Type: SEQUENCE; Schema: public; Owner: sim
--

CREATE SEQUENCE public.histories_applno_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.histories_applno_seq OWNER TO sim;

--
-- Name: histories_applno_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: sim
--

ALTER SEQUENCE public.histories_applno_seq OWNED BY public.histories.applno;


--
-- Name: histories applno; Type: DEFAULT; Schema: public; Owner: sim
--

ALTER TABLE ONLY public.histories ALTER COLUMN applno SET DEFAULT nextval('public.histories_applno_seq'::regclass);


--
-- Name: histories histories_pkey; Type: CONSTRAINT; Schema: public; Owner: sim
--

ALTER TABLE ONLY public.histories
    ADD CONSTRAINT histories_pkey PRIMARY KEY (applno);


--
-- PostgreSQL database dump complete
--

