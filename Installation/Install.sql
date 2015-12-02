--
-- PostgreSQL database dump
--

-- Dumped from database version 9.3.5
-- Dumped by pg_dump version 9.3.5
-- Started on 2015-12-01 22:30:08

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 236 (class 3079 OID 11750)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2419 (class 0 OID 0)
-- Dependencies: 236
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 170 (class 1259 OID 125789)
-- Name: ProfileData; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "ProfileData" (
    "pId" uuid NOT NULL,
    "Profile" uuid NOT NULL,
    "Name" character varying(255) NOT NULL,
    "ValueString" text,
    "ValueBinary" bytea
);


ALTER TABLE public."ProfileData" OWNER TO postgres;

--
-- TOC entry 171 (class 1259 OID 125795)
-- Name: Profiles; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Profiles" (
    "pId" uuid NOT NULL,
    "Username" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL,
    "IsAnonymous" boolean,
    "LastActivityDate" timestamp with time zone,
    "LastUpdatedDate" timestamp with time zone
);


ALTER TABLE public."Profiles" OWNER TO postgres;

--
-- TOC entry 172 (class 1259 OID 125801)
-- Name: Roles; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Roles" (
    "Rolename" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL
);


ALTER TABLE public."Roles" OWNER TO postgres;

--
-- TOC entry 173 (class 1259 OID 125807)
-- Name: Sessions; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Sessions" (
    "SessionId" character varying(80) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL,
    "Created" timestamp with time zone NOT NULL,
    "Expires" timestamp with time zone NOT NULL,
    "Timeout" integer NOT NULL,
    "Locked" boolean NOT NULL,
    "LockId" integer NOT NULL,
    "LockDate" timestamp with time zone NOT NULL,
    "Data" text,
    "Flags" integer NOT NULL
);


ALTER TABLE public."Sessions" OWNER TO postgres;

--
-- TOC entry 174 (class 1259 OID 125813)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Users" (
    "pId" uuid NOT NULL,
    "Username" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL,
    "Email" character varying(128),
    "Comment" character varying(128),
    "Password" character varying(255) NOT NULL,
    "PasswordQuestion" character varying(255),
    "PasswordAnswer" character varying(255),
    "IsApproved" boolean,
    "LastActivityDate" timestamp with time zone,
    "LastLoginDate" timestamp with time zone,
    "LastPasswordChangedDate" timestamp with time zone,
    "CreationDate" timestamp with time zone,
    "IsOnLine" boolean,
    "IsLockedOut" boolean,
    "LastLockedOutDate" timestamp with time zone,
    "FailedPasswordAttemptCount" integer,
    "FailedPasswordAttemptWindowStart" timestamp with time zone,
    "FailedPasswordAnswerAttemptCount" integer,
    "FailedPasswordAnswerAttemptWindowStart" timestamp with time zone
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 175 (class 1259 OID 125819)
-- Name: UsersInRoles; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "UsersInRoles" (
    "Username" character varying(255) NOT NULL,
    "Rolename" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL
);


ALTER TABLE public."UsersInRoles" OWNER TO postgres;

--
-- TOC entry 176 (class 1259 OID 125825)
-- Name: core; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE core (
    created_by_user_pid uuid NOT NULL,
    modified_by_user_pid uuid NOT NULL,
    disabled_by_user_pid uuid,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.core OWNER TO postgres;

--
-- TOC entry 177 (class 1259 OID 125828)
-- Name: billing_group; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE billing_group (
    id integer NOT NULL,
    title text NOT NULL,
    last_run timestamp without time zone,
    next_run timestamp without time zone NOT NULL,
    amount money NOT NULL,
    bill_to_contact_id integer NOT NULL
)
INHERITS (core);


ALTER TABLE public.billing_group OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 125834)
-- Name: billing_group_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE billing_group_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.billing_group_id_seq OWNER TO postgres;

--
-- TOC entry 2420 (class 0 OID 0)
-- Dependencies: 178
-- Name: billing_group_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE billing_group_id_seq OWNED BY billing_group.id;


--
-- TOC entry 179 (class 1259 OID 125836)
-- Name: billing_rate; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE billing_rate (
    id integer NOT NULL,
    title text NOT NULL,
    price_per_unit money NOT NULL
)
INHERITS (core);


ALTER TABLE public.billing_rate OWNER TO postgres;

--
-- TOC entry 180 (class 1259 OID 125842)
-- Name: billing_rate_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE billing_rate_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.billing_rate_id_seq OWNER TO postgres;

--
-- TOC entry 2421 (class 0 OID 0)
-- Dependencies: 180
-- Name: billing_rate_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE billing_rate_id_seq OWNED BY billing_rate.id;


--
-- TOC entry 181 (class 1259 OID 125844)
-- Name: contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE contact (
    id integer NOT NULL,
    is_organization boolean NOT NULL,
    is_our_employee boolean NOT NULL,
    nickname text,
    generation text,
    display_name_prefix text,
    surname text,
    middle_name text,
    given_name text,
    initials text,
    display_name text NOT NULL,
    email1_display_name text,
    email1_email_address text,
    email2_display_name text,
    email2_email_address text,
    email3_display_name text,
    email3_email_address text,
    fax1_display_name text,
    fax1_fax_number text,
    fax2_display_name text,
    fax2_fax_number text,
    fax3_display_name text,
    fax3_fax_number text,
    address1_display_name text,
    address1_address_street text,
    address1_address_city text,
    address1_address_state_or_province text,
    address1_address_postal_code text,
    address1_address_country text,
    address1_address_country_code text,
    address1_address_post_office_box text,
    address2_display_name text,
    address2_address_street text,
    address2_address_city text,
    address2_address_state_or_province text,
    address2_address_postal_code text,
    address2_address_country text,
    address2_address_country_code text,
    address2_address_post_office_box text,
    address3_display_name text,
    address3_address_street text,
    address3_address_city text,
    address3_address_state_or_province text,
    address3_address_postal_code text,
    address3_address_country text,
    address3_address_country_code text,
    address3_address_post_office_box text,
    telephone1_display_name text,
    telephone1_telephone_number text,
    telephone2_display_name text,
    telephone2_telephone_number text,
    telephone3_display_name text,
    telephone3_telephone_number text,
    telephone4_display_name text,
    telephone4_telephone_number text,
    telephone5_display_name text,
    telephone5_telephone_number text,
    telephone6_display_name text,
    telephone6_telephone_number text,
    telephone7_display_name text,
    telephone7_telephone_number text,
    telephone8_display_name text,
    telephone8_telephone_number text,
    telephone9_display_name text,
    telephone9_telephone_number text,
    telephone10_display_name text,
    telephone10_telephone_number text,
    birthday timestamp without time zone,
    wedding timestamp without time zone,
    title text,
    company_name text,
    department_name text,
    office_location text,
    manager_name text,
    assistant_name text,
    profession text,
    spouse_name text,
    language text,
    instant_messaging_address text,
    personal_home_page text,
    business_home_page text,
    gender text,
    referred_by_name text,
    billing_rate_id integer
)
INHERITS (core);


ALTER TABLE public.contact OWNER TO postgres;

--
-- TOC entry 182 (class 1259 OID 125850)
-- Name: contact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE contact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.contact_id_seq OWNER TO postgres;

--
-- TOC entry 2422 (class 0 OID 0)
-- Dependencies: 182
-- Name: contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE contact_id_seq OWNED BY contact.id;


--
-- TOC entry 183 (class 1259 OID 125852)
-- Name: document; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE document (
    id uuid NOT NULL,
    title text NOT NULL,
    date timestamp without time zone
)
INHERITS (core);


ALTER TABLE public.document OWNER TO postgres;

--
-- TOC entry 184 (class 1259 OID 125858)
-- Name: document_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE document_matter (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    matter_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.document_matter OWNER TO postgres;

--
-- TOC entry 185 (class 1259 OID 125861)
-- Name: document_task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE document_task (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    task_id bigint NOT NULL
)
INHERITS (core);


ALTER TABLE public.document_task OWNER TO postgres;

--
-- TOC entry 186 (class 1259 OID 125864)
-- Name: elmah_error_sequence; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE elmah_error_sequence
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.elmah_error_sequence OWNER TO postgres;

--
-- TOC entry 187 (class 1259 OID 125866)
-- Name: elmah_error; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE elmah_error (
    errorid character(36) NOT NULL,
    application character varying(60) NOT NULL,
    host character varying(50) NOT NULL,
    type character varying(100) NOT NULL,
    source character varying(60) NOT NULL,
    message character varying(500) NOT NULL,
    "User" character varying(50) NOT NULL,
    statuscode integer NOT NULL,
    timeutc timestamp without time zone NOT NULL,
    sequence integer DEFAULT nextval('elmah_error_sequence'::regclass) NOT NULL,
    allxml text NOT NULL
);


ALTER TABLE public.elmah_error OWNER TO postgres;

--
-- TOC entry 188 (class 1259 OID 125873)
-- Name: event; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event (
    id uuid NOT NULL,
    title text NOT NULL,
    allday boolean NOT NULL,
    start timestamp without time zone NOT NULL,
    "end" timestamp without time zone,
    location text,
    description text
)
INHERITS (core);


ALTER TABLE public.event OWNER TO postgres;

--
-- TOC entry 189 (class 1259 OID 125879)
-- Name: event_assigned_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_assigned_contact (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    contact_id integer NOT NULL,
    role text NOT NULL
)
INHERITS (core);


ALTER TABLE public.event_assigned_contact OWNER TO postgres;

--
-- TOC entry 190 (class 1259 OID 125885)
-- Name: event_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_matter (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    matter_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.event_matter OWNER TO postgres;

--
-- TOC entry 191 (class 1259 OID 125888)
-- Name: event_note; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_note (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    note_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.event_note OWNER TO postgres;

--
-- TOC entry 192 (class 1259 OID 125891)
-- Name: event_responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_responsible_user (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    user_pid uuid NOT NULL,
    responsibility text NOT NULL
)
INHERITS (core);


ALTER TABLE public.event_responsible_user OWNER TO postgres;

--
-- TOC entry 193 (class 1259 OID 125897)
-- Name: event_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_tag (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    tag_category_id integer,
    tag text NOT NULL
)
INHERITS (core);


ALTER TABLE public.event_tag OWNER TO postgres;

--
-- TOC entry 194 (class 1259 OID 125903)
-- Name: event_task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_task (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    task_id bigint NOT NULL
)
INHERITS (core);


ALTER TABLE public.event_task OWNER TO postgres;

--
-- TOC entry 195 (class 1259 OID 125906)
-- Name: expense; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE expense (
    id uuid NOT NULL,
    incurred timestamp without time zone NOT NULL,
    paid timestamp without time zone,
    vendor text NOT NULL,
    amount money NOT NULL,
    details text NOT NULL
)
INHERITS (core);


ALTER TABLE public.expense OWNER TO postgres;

--
-- TOC entry 196 (class 1259 OID 125912)
-- Name: expense_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE expense_matter (
    id uuid NOT NULL,
    matter_id uuid NOT NULL,
    expense_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.expense_matter OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 125915)
-- Name: external_session; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE external_session (
    id uuid NOT NULL,
    user_pid uuid NOT NULL,
    app_name text NOT NULL,
    machine_id uuid NOT NULL,
    utc_created timestamp without time zone NOT NULL,
    utc_expires timestamp without time zone NOT NULL,
    timeout integer NOT NULL
);


ALTER TABLE public.external_session OWNER TO postgres;

--
-- TOC entry 198 (class 1259 OID 125921)
-- Name: fee; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE fee (
    id uuid NOT NULL,
    incurred timestamp without time zone NOT NULL,
    amount money NOT NULL,
    details text NOT NULL
)
INHERITS (core);


ALTER TABLE public.fee OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 125927)
-- Name: fee_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE fee_matter (
    id uuid NOT NULL,
    matter_id uuid NOT NULL,
    fee_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.fee_matter OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 125930)
-- Name: form; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE form (
    id integer NOT NULL,
    title text NOT NULL,
    matter_type_id integer NOT NULL,
    path text NOT NULL
)
INHERITS (core);


ALTER TABLE public.form OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 125936)
-- Name: form_field; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE form_field (
    id integer NOT NULL,
    title text NOT NULL,
    description text
)
INHERITS (core);


ALTER TABLE public.form_field OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 125942)
-- Name: form_field_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE form_field_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.form_field_id_seq OWNER TO postgres;

--
-- TOC entry 2423 (class 0 OID 0)
-- Dependencies: 202
-- Name: form_field_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE form_field_id_seq OWNED BY form_field.id;


--
-- TOC entry 203 (class 1259 OID 125944)
-- Name: form_field_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE form_field_matter (
    id uuid NOT NULL,
    matter_id uuid NOT NULL,
    form_field_id integer NOT NULL,
    value text
)
INHERITS (core);


ALTER TABLE public.form_field_matter OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 125950)
-- Name: form_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE form_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.form_id_seq OWNER TO postgres;

--
-- TOC entry 2424 (class 0 OID 0)
-- Dependencies: 204
-- Name: form_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE form_id_seq OWNED BY form.id;


--
-- TOC entry 205 (class 1259 OID 125952)
-- Name: invoice; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE invoice (
    id uuid NOT NULL,
    bill_to_contact_id integer NOT NULL,
    date timestamp without time zone NOT NULL,
    due timestamp without time zone NOT NULL,
    subtotal money NOT NULL,
    tax_amount money NOT NULL,
    total money NOT NULL,
    external_invoice_id text,
    bill_to_name_line_1 text NOT NULL,
    bill_to_name_line_2 text,
    bill_to_address_line_1 text NOT NULL,
    bill_to_address_line_2 text,
    bill_to_city text NOT NULL,
    bill_to_state text NOT NULL,
    bill_to_zip text NOT NULL,
    matter_id uuid,
    billing_group_id integer
)
INHERITS (core);


ALTER TABLE public.invoice OWNER TO postgres;

--
-- TOC entry 206 (class 1259 OID 125958)
-- Name: invoice_expense; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE invoice_expense (
    id uuid NOT NULL,
    invoice_id uuid NOT NULL,
    expense_id uuid NOT NULL,
    amount money NOT NULL,
    details text NOT NULL
)
INHERITS (core);


ALTER TABLE public.invoice_expense OWNER TO postgres;

--
-- TOC entry 207 (class 1259 OID 125964)
-- Name: invoice_fee; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE invoice_fee (
    id uuid NOT NULL,
    invoice_id uuid NOT NULL,
    fee_id uuid NOT NULL,
    amount money NOT NULL,
    tax_amount money NOT NULL,
    details text NOT NULL
)
INHERITS (core);


ALTER TABLE public.invoice_fee OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 125970)
-- Name: invoice_time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE invoice_time (
    id uuid NOT NULL,
    invoice_id uuid NOT NULL,
    time_id uuid NOT NULL,
    details text NOT NULL,
    duration interval NOT NULL,
    price_per_hour money NOT NULL
)
INHERITS (core);


ALTER TABLE public.invoice_time OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 125976)
-- Name: matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter (
    id uuid NOT NULL,
    title text NOT NULL,
    active boolean NOT NULL,
    parent_id uuid,
    synopsis text NOT NULL,
    jurisdiction text,
    case_number text,
    lead_attorney_contact_id integer,
    bill_to_contact_id integer,
    minimum_charge money,
    estimated_charge money,
    maximum_charge money,
    default_billing_rate_id integer,
    billing_group_id integer,
    override_matter_rate_with_employee_rate boolean DEFAULT false NOT NULL,
    matter_type_id integer
)
INHERITS (core);


ALTER TABLE public.matter OWNER TO postgres;

--
-- TOC entry 210 (class 1259 OID 125983)
-- Name: matter_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_contact (
    id integer NOT NULL,
    matter_id uuid NOT NULL,
    contact_id integer NOT NULL,
    role text NOT NULL
)
INHERITS (core);


ALTER TABLE public.matter_contact OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 125989)
-- Name: matter_contact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE matter_contact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.matter_contact_id_seq OWNER TO postgres;

--
-- TOC entry 2425 (class 0 OID 0)
-- Dependencies: 211
-- Name: matter_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_contact_id_seq OWNED BY matter_contact.id;


--
-- TOC entry 212 (class 1259 OID 125991)
-- Name: matter_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_tag (
    id uuid NOT NULL,
    matter_id uuid NOT NULL,
    tag_category_id integer,
    tag text NOT NULL
)
INHERITS (core);


ALTER TABLE public.matter_tag OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 125997)
-- Name: matter_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_type (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.matter_type OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 126003)
-- Name: matter_type_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE matter_type_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.matter_type_id_seq OWNER TO postgres;

--
-- TOC entry 2426 (class 0 OID 0)
-- Dependencies: 214
-- Name: matter_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_type_id_seq OWNED BY matter_type.id;


--
-- TOC entry 215 (class 1259 OID 126005)
-- Name: note; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE note (
    id uuid NOT NULL,
    title text NOT NULL,
    body text NOT NULL,
    "timestamp" timestamp without time zone NOT NULL
)
INHERITS (core);


ALTER TABLE public.note OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 126011)
-- Name: note_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE note_matter (
    id uuid NOT NULL,
    note_id uuid NOT NULL,
    matter_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.note_matter OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 126014)
-- Name: note_notification; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE note_notification (
    id uuid NOT NULL,
    contact_id integer NOT NULL,
    note_id uuid NOT NULL,
    cleared timestamp without time zone
)
INHERITS (core);


ALTER TABLE public.note_notification OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 126017)
-- Name: note_task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE note_task (
    id uuid NOT NULL,
    note_id uuid NOT NULL,
    task_id bigint NOT NULL
)
INHERITS (core);


ALTER TABLE public.note_task OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 126020)
-- Name: responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE responsible_user (
    id integer NOT NULL,
    matter_id uuid NOT NULL,
    user_pid uuid NOT NULL,
    responsibility text NOT NULL
)
INHERITS (core);


ALTER TABLE public.responsible_user OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 126026)
-- Name: responsible_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE responsible_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.responsible_user_id_seq OWNER TO postgres;

--
-- TOC entry 2427 (class 0 OID 0)
-- Dependencies: 220
-- Name: responsible_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE responsible_user_id_seq OWNED BY responsible_user.id;


--
-- TOC entry 221 (class 1259 OID 126028)
-- Name: tag_category; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tag_category (
    id integer NOT NULL,
    name text NOT NULL
)
INHERITS (core);


ALTER TABLE public.tag_category OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 126034)
-- Name: tag_category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE tag_category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tag_category_id_seq OWNER TO postgres;

--
-- TOC entry 2428 (class 0 OID 0)
-- Dependencies: 222
-- Name: tag_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE tag_category_id_seq OWNED BY tag_category.id;


--
-- TOC entry 223 (class 1259 OID 126036)
-- Name: tag_filter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tag_filter (
    id bigint NOT NULL,
    user_pid uuid NOT NULL,
    category text,
    tag text NOT NULL
)
INHERITS (core);


ALTER TABLE public.tag_filter OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 126042)
-- Name: tag_filter_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE tag_filter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tag_filter_id_seq OWNER TO postgres;

--
-- TOC entry 2429 (class 0 OID 0)
-- Dependencies: 224
-- Name: tag_filter_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE tag_filter_id_seq OWNED BY tag_filter.id;


--
-- TOC entry 225 (class 1259 OID 126044)
-- Name: task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task (
    id bigint NOT NULL,
    title text NOT NULL,
    description text NOT NULL,
    projected_start timestamp without time zone,
    due_date timestamp without time zone,
    projected_end timestamp without time zone,
    actual_end timestamp without time zone,
    parent_id bigint,
    is_grouping_task boolean NOT NULL,
    sequential_predecessor_id bigint,
    active boolean NOT NULL
)
INHERITS (core);


ALTER TABLE public.task OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 126050)
-- Name: task_assigned_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_assigned_contact (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    contact_id integer NOT NULL,
    assignment_type smallint DEFAULT 1 NOT NULL
)
INHERITS (core);


ALTER TABLE public.task_assigned_contact OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 126054)
-- Name: task_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE task_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.task_id_seq OWNER TO postgres;

--
-- TOC entry 2430 (class 0 OID 0)
-- Dependencies: 227
-- Name: task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_id_seq OWNED BY task.id;


--
-- TOC entry 228 (class 1259 OID 126056)
-- Name: task_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_matter (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    matter_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.task_matter OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 126059)
-- Name: task_responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_responsible_user (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    user_pid uuid NOT NULL,
    responsibility text NOT NULL
)
INHERITS (core);


ALTER TABLE public.task_responsible_user OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 126065)
-- Name: task_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_tag (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    tag_category_id integer,
    tag text NOT NULL
)
INHERITS (core);


ALTER TABLE public.task_tag OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 126071)
-- Name: task_template; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_template (
    id integer NOT NULL,
    task_template_title text NOT NULL,
    title text,
    description text,
    active boolean NOT NULL,
    projected_start text,
    due_date text,
    actual_end text,
    projected_end text
)
INHERITS (core);


ALTER TABLE public.task_template OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 126077)
-- Name: task_template_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE task_template_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.task_template_id_seq OWNER TO postgres;

--
-- TOC entry 2431 (class 0 OID 0)
-- Dependencies: 232
-- Name: task_template_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_template_id_seq OWNED BY task_template.id;


--
-- TOC entry 233 (class 1259 OID 126079)
-- Name: task_time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_time (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    time_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.task_time OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 126082)
-- Name: time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "time" (
    id uuid NOT NULL,
    start timestamp without time zone NOT NULL,
    stop timestamp without time zone,
    worker_contact_id integer NOT NULL,
    details text,
    billable boolean DEFAULT false NOT NULL
)
INHERITS (core);


ALTER TABLE public."time" OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 126089)
-- Name: version; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE version (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    version_number integer NOT NULL,
    mime text NOT NULL,
    filename text NOT NULL,
    extension text NOT NULL,
    size bigint NOT NULL,
    md5 text NOT NULL
)
INHERITS (core);


ALTER TABLE public.version OWNER TO postgres;

--
-- TOC entry 2094 (class 2604 OID 126095)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_group ALTER COLUMN id SET DEFAULT nextval('billing_group_id_seq'::regclass);


--
-- TOC entry 2095 (class 2604 OID 126096)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_rate ALTER COLUMN id SET DEFAULT nextval('billing_rate_id_seq'::regclass);


--
-- TOC entry 2096 (class 2604 OID 126097)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact ALTER COLUMN id SET DEFAULT nextval('contact_id_seq'::regclass);


--
-- TOC entry 2098 (class 2604 OID 126098)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY form ALTER COLUMN id SET DEFAULT nextval('form_id_seq'::regclass);


--
-- TOC entry 2099 (class 2604 OID 126099)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY form_field ALTER COLUMN id SET DEFAULT nextval('form_field_id_seq'::regclass);


--
-- TOC entry 2101 (class 2604 OID 126100)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact ALTER COLUMN id SET DEFAULT nextval('matter_contact_id_seq'::regclass);


--
-- TOC entry 2102 (class 2604 OID 126101)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_type ALTER COLUMN id SET DEFAULT nextval('matter_type_id_seq'::regclass);


--
-- TOC entry 2103 (class 2604 OID 126102)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user ALTER COLUMN id SET DEFAULT nextval('responsible_user_id_seq'::regclass);


--
-- TOC entry 2104 (class 2604 OID 126103)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category ALTER COLUMN id SET DEFAULT nextval('tag_category_id_seq'::regclass);


--
-- TOC entry 2105 (class 2604 OID 126104)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_filter ALTER COLUMN id SET DEFAULT nextval('tag_filter_id_seq'::regclass);


--
-- TOC entry 2106 (class 2604 OID 126105)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task ALTER COLUMN id SET DEFAULT nextval('task_id_seq'::regclass);


--
-- TOC entry 2108 (class 2604 OID 126106)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_template ALTER COLUMN id SET DEFAULT nextval('task_template_id_seq'::regclass);


--
-- TOC entry 2216 (class 2606 OID 129389)
-- Name: UQ_task_matter_Task_Matter; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "UQ_task_matter_Task_Matter" UNIQUE (task_id, matter_id);


--
-- TOC entry 2124 (class 2606 OID 129391)
-- Name: Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("pId");


--
-- TOC entry 2132 (class 2606 OID 129393)
-- Name: billing_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_group
    ADD CONSTRAINT billing_group_pkey PRIMARY KEY (id);


--
-- TOC entry 2134 (class 2606 OID 129395)
-- Name: billing_rates_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_rate
    ADD CONSTRAINT billing_rates_pkey PRIMARY KEY (id);


--
-- TOC entry 2136 (class 2606 OID 129397)
-- Name: billing_rates_title_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_rate
    ADD CONSTRAINT billing_rates_title_unique UNIQUE (title);


--
-- TOC entry 2138 (class 2606 OID 129399)
-- Name: contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2142 (class 2606 OID 129401)
-- Name: document_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT document_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2140 (class 2606 OID 129403)
-- Name: document_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY document
    ADD CONSTRAINT document_pkey PRIMARY KEY (id);


--
-- TOC entry 2144 (class 2606 OID 129405)
-- Name: document_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT document_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2151 (class 2606 OID 129407)
-- Name: event_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT event_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2153 (class 2606 OID 129409)
-- Name: event_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT event_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2155 (class 2606 OID 129411)
-- Name: event_note_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT event_note_pkey PRIMARY KEY (id);


--
-- TOC entry 2149 (class 2606 OID 129413)
-- Name: event_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event
    ADD CONSTRAINT event_pkey PRIMARY KEY (id);


--
-- TOC entry 2157 (class 2606 OID 129415)
-- Name: event_responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT event_responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2159 (class 2606 OID 129417)
-- Name: event_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT event_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2161 (class 2606 OID 129419)
-- Name: event_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT event_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2165 (class 2606 OID 129421)
-- Name: expense_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT expense_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2163 (class 2606 OID 129423)
-- Name: expense_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY expense
    ADD CONSTRAINT expense_pkey PRIMARY KEY (id);


--
-- TOC entry 2167 (class 2606 OID 129425)
-- Name: external_session_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY external_session
    ADD CONSTRAINT external_session_pkey PRIMARY KEY (id);


--
-- TOC entry 2171 (class 2606 OID 129427)
-- Name: fee_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT fee_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2169 (class 2606 OID 129429)
-- Name: fee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY fee
    ADD CONSTRAINT fee_pkey PRIMARY KEY (id);


--
-- TOC entry 2175 (class 2606 OID 129431)
-- Name: form_field_PK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY form_field
    ADD CONSTRAINT "form_field_PK" PRIMARY KEY (id);


--
-- TOC entry 2179 (class 2606 OID 129433)
-- Name: form_field_matter_PK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY form_field_matter
    ADD CONSTRAINT "form_field_matter_PK" PRIMARY KEY (id);


--
-- TOC entry 2177 (class 2606 OID 129435)
-- Name: form_field_title_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY form_field
    ADD CONSTRAINT form_field_title_unique UNIQUE (title);


--
-- TOC entry 2173 (class 2606 OID 129437)
-- Name: form_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY form
    ADD CONSTRAINT form_pkey PRIMARY KEY (id);


--
-- TOC entry 2183 (class 2606 OID 129439)
-- Name: invoice_expense_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT invoice_expense_pkey PRIMARY KEY (id);


--
-- TOC entry 2185 (class 2606 OID 129441)
-- Name: invoice_fee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT invoice_fee_pkey PRIMARY KEY (id);


--
-- TOC entry 2181 (class 2606 OID 129443)
-- Name: invoice_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT invoice_pkey PRIMARY KEY (id);


--
-- TOC entry 2187 (class 2606 OID 129445)
-- Name: invoice_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT invoice_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2191 (class 2606 OID 129447)
-- Name: matter_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT matter_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2189 (class 2606 OID 129449)
-- Name: matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2193 (class 2606 OID 129451)
-- Name: matter_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT matter_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2195 (class 2606 OID 129453)
-- Name: matter_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_type
    ADD CONSTRAINT matter_type_pkey PRIMARY KEY (id);


--
-- TOC entry 2199 (class 2606 OID 129455)
-- Name: note_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT note_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2201 (class 2606 OID 129457)
-- Name: note_notification_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT note_notification_pkey PRIMARY KEY (id);


--
-- TOC entry 2197 (class 2606 OID 129459)
-- Name: note_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note
    ADD CONSTRAINT note_pkey PRIMARY KEY (id);


--
-- TOC entry 2203 (class 2606 OID 129461)
-- Name: note_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT note_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2147 (class 2606 OID 129463)
-- Name: pk_elmah_error; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY elmah_error
    ADD CONSTRAINT pk_elmah_error PRIMARY KEY (errorid);


--
-- TOC entry 2111 (class 2606 OID 129465)
-- Name: profiledata_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_pkey PRIMARY KEY ("pId");


--
-- TOC entry 2113 (class 2606 OID 129467)
-- Name: profiledata_profile_name_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_name_unique UNIQUE ("Profile", "Name");


--
-- TOC entry 2116 (class 2606 OID 129469)
-- Name: profiles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_pkey PRIMARY KEY ("pId");


--
-- TOC entry 2118 (class 2606 OID 129471)
-- Name: profiles_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 2205 (class 2606 OID 129473)
-- Name: responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2120 (class 2606 OID 129475)
-- Name: roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Roles"
    ADD CONSTRAINT roles_pkey PRIMARY KEY ("Rolename", "ApplicationName");


--
-- TOC entry 2122 (class 2606 OID 129477)
-- Name: sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Sessions"
    ADD CONSTRAINT sessions_pkey PRIMARY KEY ("SessionId", "ApplicationName");


--
-- TOC entry 2207 (class 2606 OID 129479)
-- Name: tag_category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT tag_category_pkey PRIMARY KEY (id);


--
-- TOC entry 2210 (class 2606 OID 129481)
-- Name: tag_filter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tag_filter
    ADD CONSTRAINT tag_filter_pkey PRIMARY KEY (id);


--
-- TOC entry 2214 (class 2606 OID 129483)
-- Name: task_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT task_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2218 (class 2606 OID 129485)
-- Name: task_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT task_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2212 (class 2606 OID 129487)
-- Name: task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task
    ADD CONSTRAINT task_pkey PRIMARY KEY (id);


--
-- TOC entry 2220 (class 2606 OID 129489)
-- Name: task_responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT task_responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2222 (class 2606 OID 129491)
-- Name: task_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT task_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2224 (class 2606 OID 129493)
-- Name: task_template_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_template
    ADD CONSTRAINT task_template_pkey PRIMARY KEY (id);


--
-- TOC entry 2226 (class 2606 OID 129495)
-- Name: task_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT task_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2228 (class 2606 OID 129497)
-- Name: time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT time_pkey PRIMARY KEY (id);


--
-- TOC entry 2128 (class 2606 OID 129499)
-- Name: users_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT users_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 2130 (class 2606 OID 129501)
-- Name: usersinroles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_pkey PRIMARY KEY ("Username", "Rolename", "ApplicationName");


--
-- TOC entry 2230 (class 2606 OID 129503)
-- Name: version_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY version
    ADD CONSTRAINT version_pkey PRIMARY KEY (id);


--
-- TOC entry 2145 (class 1259 OID 129504)
-- Name: ix_elmah_error_app_time_seq; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX ix_elmah_error_app_time_seq ON elmah_error USING btree (application, timeutc DESC, sequence DESC);


--
-- TOC entry 2114 (class 1259 OID 129505)
-- Name: profiles_isanonymous_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX profiles_isanonymous_index ON "Profiles" USING btree ("IsAnonymous");


--
-- TOC entry 2208 (class 1259 OID 129506)
-- Name: uidx_tagcategory_name; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_tagcategory_name ON tag_category USING btree (name);


--
-- TOC entry 2125 (class 1259 OID 129507)
-- Name: users_email_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX users_email_index ON "Users" USING btree ("Email");


--
-- TOC entry 2126 (class 1259 OID 129508)
-- Name: users_islockedout_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX users_islockedout_index ON "Users" USING btree ("IsLockedOut");


--
-- TOC entry 2238 (class 2606 OID 129509)
-- Name: FK_billing_group_contact_BillToContactId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_group
    ADD CONSTRAINT "FK_billing_group_contact_BillToContactId_Id" FOREIGN KEY (bill_to_contact_id) REFERENCES contact(id);


--
-- TOC entry 2239 (class 2606 OID 129514)
-- Name: FK_contact_billing_rate_BillingRateId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_billing_rate_BillingRateId" FOREIGN KEY (billing_rate_id) REFERENCES billing_rate(id);


--
-- TOC entry 2235 (class 2606 OID 129519)
-- Name: FK_core_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_CreatedByUserId" FOREIGN KEY (created_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2236 (class 2606 OID 129524)
-- Name: FK_core_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2237 (class 2606 OID 129529)
-- Name: FK_core_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2240 (class 2606 OID 129534)
-- Name: FK_document_matter_document_DocumentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT "FK_document_matter_document_DocumentId" FOREIGN KEY (document_id) REFERENCES document(id);


--
-- TOC entry 2241 (class 2606 OID 129539)
-- Name: FK_document_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT "FK_document_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2242 (class 2606 OID 129544)
-- Name: FK_document_task_document_DocumentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT "FK_document_task_document_DocumentId" FOREIGN KEY (document_id) REFERENCES document(id);


--
-- TOC entry 2243 (class 2606 OID 129549)
-- Name: FK_document_task_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT "FK_document_task_matter_MatterId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2244 (class 2606 OID 129554)
-- Name: FK_event_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT "FK_event_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2245 (class 2606 OID 129559)
-- Name: FK_event_assigned_contact_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT "FK_event_assigned_contact_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2246 (class 2606 OID 129564)
-- Name: FK_event_matter_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT "FK_event_matter_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2247 (class 2606 OID 129569)
-- Name: FK_event_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT "FK_event_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2248 (class 2606 OID 129574)
-- Name: FK_event_note_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT "FK_event_note_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2249 (class 2606 OID 129579)
-- Name: FK_event_note_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT "FK_event_note_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2250 (class 2606 OID 129584)
-- Name: FK_event_responsible_user_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT "FK_event_responsible_user_task_TaskId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2251 (class 2606 OID 129589)
-- Name: FK_event_responsible_user_user_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT "FK_event_responsible_user_user_MatterId" FOREIGN KEY (user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2252 (class 2606 OID 129594)
-- Name: FK_event_tag_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT "FK_event_tag_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2253 (class 2606 OID 129599)
-- Name: FK_event_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT "FK_event_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2254 (class 2606 OID 129604)
-- Name: FK_event_task_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT "FK_event_task_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2255 (class 2606 OID 129609)
-- Name: FK_event_task_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT "FK_event_task_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2256 (class 2606 OID 129614)
-- Name: FK_expense_matter_ExpenseId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT "FK_expense_matter_ExpenseId" FOREIGN KEY (expense_id) REFERENCES expense(id);


--
-- TOC entry 2257 (class 2606 OID 129619)
-- Name: FK_expense_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT "FK_expense_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2258 (class 2606 OID 129624)
-- Name: FK_external_session_users_UserPId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY external_session
    ADD CONSTRAINT "FK_external_session_users_UserPId" FOREIGN KEY (user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2264 (class 2606 OID 129629)
-- Name: FK_invoice_billing_group_BillingGroupIp; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "FK_invoice_billing_group_BillingGroupIp" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2265 (class 2606 OID 129634)
-- Name: FK_invoice_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "FK_invoice_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2273 (class 2606 OID 129639)
-- Name: FK_matter_billing_group_BillingGroupId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_group_BillingGroupId" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2274 (class 2606 OID 129644)
-- Name: FK_matter_billing_group_BillingGroupId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_group_BillingGroupId_Id" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2275 (class 2606 OID 129649)
-- Name: FK_matter_billing_rate_DefaultBillingRateId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_rate_DefaultBillingRateId_Id" FOREIGN KEY (default_billing_rate_id) REFERENCES billing_rate(id);


--
-- TOC entry 2276 (class 2606 OID 129654)
-- Name: FK_matter_contact_lead_attorney_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_contact_lead_attorney_contact_id" FOREIGN KEY (lead_attorney_contact_id) REFERENCES contact(id);


--
-- TOC entry 2279 (class 2606 OID 129659)
-- Name: FK_matter_contact_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2280 (class 2606 OID 129664)
-- Name: FK_matter_contact_user_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2277 (class 2606 OID 129669)
-- Name: FK_matter_matter_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_ParentId" FOREIGN KEY (parent_id) REFERENCES matter(id);


--
-- TOC entry 2278 (class 2606 OID 129674)
-- Name: FK_matter_matter_type_MatterTypeId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_type_MatterTypeId_Id" FOREIGN KEY (matter_type_id) REFERENCES matter_type(id);


--
-- TOC entry 2281 (class 2606 OID 129679)
-- Name: FK_matter_tag_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2282 (class 2606 OID 129684)
-- Name: FK_matter_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2283 (class 2606 OID 129689)
-- Name: FK_note_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2284 (class 2606 OID 129694)
-- Name: FK_note_matter_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2287 (class 2606 OID 129699)
-- Name: FK_note_task_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2288 (class 2606 OID 129704)
-- Name: FK_note_task_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2289 (class 2606 OID 129709)
-- Name: FK_responsible_user_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2290 (class 2606 OID 129714)
-- Name: FK_responsible_user_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_UserId" FOREIGN KEY (user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2291 (class 2606 OID 129719)
-- Name: FK_tag_filter_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_filter
    ADD CONSTRAINT "FK_tag_filter_user_UserId" FOREIGN KEY (user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2294 (class 2606 OID 129724)
-- Name: FK_task_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2295 (class 2606 OID 129729)
-- Name: FK_task_assigned_contact_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2296 (class 2606 OID 129734)
-- Name: FK_task_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2297 (class 2606 OID 129739)
-- Name: FK_task_matter_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2298 (class 2606 OID 129744)
-- Name: FK_task_responsible_user_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2299 (class 2606 OID 129749)
-- Name: FK_task_responsible_user_user_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_MatterId" FOREIGN KEY (user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2300 (class 2606 OID 129754)
-- Name: FK_task_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2301 (class 2606 OID 129759)
-- Name: FK_task_tag_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2292 (class 2606 OID 129764)
-- Name: FK_task_task_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_ParentId" FOREIGN KEY (parent_id) REFERENCES task(id);


--
-- TOC entry 2293 (class 2606 OID 129769)
-- Name: FK_task_task_SequentialPredecessorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_SequentialPredecessorId" FOREIGN KEY (sequential_predecessor_id) REFERENCES task(id);


--
-- TOC entry 2302 (class 2606 OID 129774)
-- Name: FK_task_time_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2303 (class 2606 OID 129779)
-- Name: FK_task_time_user_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2304 (class 2606 OID 129784)
-- Name: FK_time_user_WorkerContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_WorkerContactId" FOREIGN KEY (worker_contact_id) REFERENCES contact(id);


--
-- TOC entry 2259 (class 2606 OID 129789)
-- Name: fee_matter_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT "fee_matter_FeeId" FOREIGN KEY (fee_id) REFERENCES fee(id);


--
-- TOC entry 2260 (class 2606 OID 129794)
-- Name: fee_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT "fee_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2262 (class 2606 OID 129799)
-- Name: form_field_matter_form_field_FormFieldId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY form_field_matter
    ADD CONSTRAINT "form_field_matter_form_field_FormFieldId" FOREIGN KEY (form_field_id) REFERENCES form_field(id);


--
-- TOC entry 2263 (class 2606 OID 129804)
-- Name: form_field_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY form_field_matter
    ADD CONSTRAINT "form_field_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2261 (class 2606 OID 129809)
-- Name: form_matter_type_MatterTypeId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY form
    ADD CONSTRAINT "form_matter_type_MatterTypeId_Id" FOREIGN KEY (matter_type_id) REFERENCES matter_type(id);


--
-- TOC entry 2266 (class 2606 OID 129814)
-- Name: invoice_contact_BillToContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "invoice_contact_BillToContactId" FOREIGN KEY (bill_to_contact_id) REFERENCES contact(id);


--
-- TOC entry 2267 (class 2606 OID 129819)
-- Name: invoice_expense_expense_ExpenseId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT "invoice_expense_expense_ExpenseId" FOREIGN KEY (expense_id) REFERENCES expense(id);


--
-- TOC entry 2268 (class 2606 OID 129824)
-- Name: invoice_expense_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT "invoice_expense_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2269 (class 2606 OID 129829)
-- Name: invoice_fee_fee_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT "invoice_fee_fee_FeeId" FOREIGN KEY (fee_id) REFERENCES fee(id);


--
-- TOC entry 2270 (class 2606 OID 129834)
-- Name: invoice_fee_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT "invoice_fee_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2271 (class 2606 OID 129839)
-- Name: invoice_time_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT "invoice_time_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2272 (class 2606 OID 129844)
-- Name: invoice_time_time_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT "invoice_time_time_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2285 (class 2606 OID 129849)
-- Name: note_notification_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT "note_notification_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2286 (class 2606 OID 129854)
-- Name: note_notification_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT "note_notification_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2231 (class 2606 OID 129859)
-- Name: profiledata_profile_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_fkey FOREIGN KEY ("Profile") REFERENCES "Profiles"("pId") ON DELETE CASCADE;


--
-- TOC entry 2232 (class 2606 OID 129864)
-- Name: profiles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2233 (class 2606 OID 129869)
-- Name: usersinroles_rolename_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_rolename_fkey FOREIGN KEY ("Rolename", "ApplicationName") REFERENCES "Roles"("Rolename", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2234 (class 2606 OID 129874)
-- Name: usersinroles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2418 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2015-12-01 22:30:09

--
-- PostgreSQL database dump complete
--

