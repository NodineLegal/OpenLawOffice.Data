--
-- PostgreSQL database dump
--

-- Dumped from database version 9.3.5
-- Dumped by pg_dump version 9.3.5
-- Started on 2016-02-21 14:28:57

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 217 (class 3079 OID 11750)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2273 (class 0 OID 0)
-- Dependencies: 217
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
-- TOC entry 2274 (class 0 OID 0)
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
-- TOC entry 2275 (class 0 OID 0)
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
    billing_rate_id integer,
    address1_address_line2 text,
    address2_address_line2 text,
    address3_address_line2 text,
    bar_number text
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
-- TOC entry 2276 (class 0 OID 0)
-- Dependencies: 182
-- Name: contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE contact_id_seq OWNED BY contact.id;


--
-- TOC entry 216 (class 1259 OID 134036)
-- Name: court_geographical_jurisdiction; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE court_geographical_jurisdiction (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.court_geographical_jurisdiction OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 134034)
-- Name: court_geographical_jurisdiction_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE court_geographical_jurisdiction_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.court_geographical_jurisdiction_id_seq OWNER TO postgres;

--
-- TOC entry 2277 (class 0 OID 0)
-- Dependencies: 215
-- Name: court_geographical_jurisdiction_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE court_geographical_jurisdiction_id_seq OWNED BY court_geographical_jurisdiction.id;


--
-- TOC entry 214 (class 1259 OID 134025)
-- Name: court_sitting_in_city; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE court_sitting_in_city (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.court_sitting_in_city OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 134023)
-- Name: court_sitting_in_city_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE court_sitting_in_city_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.court_sitting_in_city_id_seq OWNER TO postgres;

--
-- TOC entry 2278 (class 0 OID 0)
-- Dependencies: 213
-- Name: court_sitting_in_city_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE court_sitting_in_city_id_seq OWNED BY court_sitting_in_city.id;


--
-- TOC entry 212 (class 1259 OID 134014)
-- Name: court_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE court_type (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.court_type OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 134012)
-- Name: court_type_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE court_type_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.court_type_id_seq OWNER TO postgres;

--
-- TOC entry 2279 (class 0 OID 0)
-- Dependencies: 211
-- Name: court_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE court_type_id_seq OWNED BY court_type.id;


--
-- TOC entry 183 (class 1259 OID 125864)
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
-- TOC entry 184 (class 1259 OID 125866)
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
-- TOC entry 185 (class 1259 OID 125906)
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
-- TOC entry 186 (class 1259 OID 125912)
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
-- TOC entry 187 (class 1259 OID 125915)
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
-- TOC entry 188 (class 1259 OID 125921)
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
-- TOC entry 189 (class 1259 OID 125927)
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
-- TOC entry 190 (class 1259 OID 125952)
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
-- TOC entry 191 (class 1259 OID 125958)
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
-- TOC entry 192 (class 1259 OID 125964)
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
-- TOC entry 193 (class 1259 OID 125970)
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
-- TOC entry 194 (class 1259 OID 125976)
-- Name: matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter (
    id uuid NOT NULL,
    title text NOT NULL,
    active boolean NOT NULL,
    parent_id uuid,
    synopsis text NOT NULL,
    case_number text,
    bill_to_contact_id integer,
    minimum_charge money,
    estimated_charge money,
    maximum_charge money,
    default_billing_rate_id integer,
    billing_group_id integer,
    override_matter_rate_with_employee_rate boolean DEFAULT false NOT NULL,
    matter_type_id integer,
    attorney_for_party_title text,
    court_type_id integer,
    court_geographical_jurisdiction_id integer,
    court_sitting_in_city_id integer,
    caption_plaintiff_or_subject_short text,
    caption_plaintiff_or_subject_regular text,
    caption_plaintiff_or_subject_long text,
    caption_other_party_short text,
    caption_other_party_regular text,
    caption_other_party_long text
)
INHERITS (core);


ALTER TABLE public.matter OWNER TO postgres;

--
-- TOC entry 195 (class 1259 OID 125983)
-- Name: matter_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_contact (
    id integer NOT NULL,
    matter_id uuid NOT NULL,
    contact_id integer NOT NULL,
    is_client boolean,
    is_client_contact boolean,
    is_appointed boolean,
    is_party boolean,
    party_title text,
    is_judge boolean,
    is_witness boolean,
    is_attorney boolean,
    attorney_for_contact_id integer,
    is_lead_attorney boolean,
    is_support_staff boolean,
    support_staff_for_contact_id integer,
    is_third_party_payor boolean,
    third_party_payor_for_contact_id integer
)
INHERITS (core);


ALTER TABLE public.matter_contact OWNER TO postgres;

--
-- TOC entry 196 (class 1259 OID 125989)
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
-- TOC entry 2280 (class 0 OID 0)
-- Dependencies: 196
-- Name: matter_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_contact_id_seq OWNED BY matter_contact.id;


--
-- TOC entry 197 (class 1259 OID 125997)
-- Name: matter_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_type (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.matter_type OWNER TO postgres;

--
-- TOC entry 198 (class 1259 OID 126003)
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
-- TOC entry 2281 (class 0 OID 0)
-- Dependencies: 198
-- Name: matter_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_type_id_seq OWNED BY matter_type.id;


--
-- TOC entry 199 (class 1259 OID 126005)
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
-- TOC entry 200 (class 1259 OID 126011)
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
-- TOC entry 201 (class 1259 OID 126014)
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
-- TOC entry 202 (class 1259 OID 126017)
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
-- TOC entry 203 (class 1259 OID 126044)
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
-- TOC entry 204 (class 1259 OID 126050)
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
-- TOC entry 205 (class 1259 OID 126054)
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
-- TOC entry 2282 (class 0 OID 0)
-- Dependencies: 205
-- Name: task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_id_seq OWNED BY task.id;


--
-- TOC entry 206 (class 1259 OID 126056)
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
-- TOC entry 207 (class 1259 OID 126071)
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
-- TOC entry 208 (class 1259 OID 126077)
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
-- TOC entry 2283 (class 0 OID 0)
-- Dependencies: 208
-- Name: task_template_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_template_id_seq OWNED BY task_template.id;


--
-- TOC entry 209 (class 1259 OID 126079)
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
-- TOC entry 210 (class 1259 OID 126082)
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
-- TOC entry 2010 (class 2604 OID 126095)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_group ALTER COLUMN id SET DEFAULT nextval('billing_group_id_seq'::regclass);


--
-- TOC entry 2011 (class 2604 OID 126096)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_rate ALTER COLUMN id SET DEFAULT nextval('billing_rate_id_seq'::regclass);


--
-- TOC entry 2012 (class 2604 OID 126097)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact ALTER COLUMN id SET DEFAULT nextval('contact_id_seq'::regclass);


--
-- TOC entry 2023 (class 2604 OID 134039)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY court_geographical_jurisdiction ALTER COLUMN id SET DEFAULT nextval('court_geographical_jurisdiction_id_seq'::regclass);


--
-- TOC entry 2022 (class 2604 OID 134028)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY court_sitting_in_city ALTER COLUMN id SET DEFAULT nextval('court_sitting_in_city_id_seq'::regclass);


--
-- TOC entry 2021 (class 2604 OID 134017)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY court_type ALTER COLUMN id SET DEFAULT nextval('court_type_id_seq'::regclass);


--
-- TOC entry 2015 (class 2604 OID 126100)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact ALTER COLUMN id SET DEFAULT nextval('matter_contact_id_seq'::regclass);


--
-- TOC entry 2016 (class 2604 OID 126101)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_type ALTER COLUMN id SET DEFAULT nextval('matter_type_id_seq'::regclass);


--
-- TOC entry 2017 (class 2604 OID 126105)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task ALTER COLUMN id SET DEFAULT nextval('task_id_seq'::regclass);


--
-- TOC entry 2019 (class 2604 OID 126106)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_template ALTER COLUMN id SET DEFAULT nextval('task_template_id_seq'::regclass);


--
-- TOC entry 2093 (class 2606 OID 129389)
-- Name: UQ_task_matter_Task_Matter; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "UQ_task_matter_Task_Matter" UNIQUE (task_id, matter_id);


--
-- TOC entry 2038 (class 2606 OID 129391)
-- Name: Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("pId");


--
-- TOC entry 2046 (class 2606 OID 129393)
-- Name: billing_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_group
    ADD CONSTRAINT billing_group_pkey PRIMARY KEY (id);


--
-- TOC entry 2048 (class 2606 OID 129395)
-- Name: billing_rates_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_rate
    ADD CONSTRAINT billing_rates_pkey PRIMARY KEY (id);


--
-- TOC entry 2050 (class 2606 OID 129397)
-- Name: billing_rates_title_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_rate
    ADD CONSTRAINT billing_rates_title_unique UNIQUE (title);


--
-- TOC entry 2052 (class 2606 OID 129399)
-- Name: contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2107 (class 2606 OID 134044)
-- Name: court_geographical_jurisdiction_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY court_geographical_jurisdiction
    ADD CONSTRAINT court_geographical_jurisdiction_pkey PRIMARY KEY (id);


--
-- TOC entry 2105 (class 2606 OID 134033)
-- Name: court_sitting_in_city_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY court_sitting_in_city
    ADD CONSTRAINT court_sitting_in_city_pkey PRIMARY KEY (id);


--
-- TOC entry 2103 (class 2606 OID 134022)
-- Name: court_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY court_type
    ADD CONSTRAINT court_type_pkey PRIMARY KEY (id);


--
-- TOC entry 2059 (class 2606 OID 129421)
-- Name: expense_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT expense_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2057 (class 2606 OID 129423)
-- Name: expense_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY expense
    ADD CONSTRAINT expense_pkey PRIMARY KEY (id);


--
-- TOC entry 2061 (class 2606 OID 129425)
-- Name: external_session_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY external_session
    ADD CONSTRAINT external_session_pkey PRIMARY KEY (id);


--
-- TOC entry 2065 (class 2606 OID 129427)
-- Name: fee_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT fee_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2063 (class 2606 OID 129429)
-- Name: fee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY fee
    ADD CONSTRAINT fee_pkey PRIMARY KEY (id);


--
-- TOC entry 2069 (class 2606 OID 129439)
-- Name: invoice_expense_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT invoice_expense_pkey PRIMARY KEY (id);


--
-- TOC entry 2071 (class 2606 OID 129441)
-- Name: invoice_fee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT invoice_fee_pkey PRIMARY KEY (id);


--
-- TOC entry 2067 (class 2606 OID 129443)
-- Name: invoice_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT invoice_pkey PRIMARY KEY (id);


--
-- TOC entry 2073 (class 2606 OID 129445)
-- Name: invoice_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT invoice_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2077 (class 2606 OID 129447)
-- Name: matter_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT matter_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2075 (class 2606 OID 129449)
-- Name: matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2079 (class 2606 OID 129453)
-- Name: matter_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_type
    ADD CONSTRAINT matter_type_pkey PRIMARY KEY (id);


--
-- TOC entry 2083 (class 2606 OID 129455)
-- Name: note_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT note_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2085 (class 2606 OID 129457)
-- Name: note_notification_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT note_notification_pkey PRIMARY KEY (id);


--
-- TOC entry 2081 (class 2606 OID 129459)
-- Name: note_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note
    ADD CONSTRAINT note_pkey PRIMARY KEY (id);


--
-- TOC entry 2087 (class 2606 OID 129461)
-- Name: note_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT note_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2055 (class 2606 OID 129463)
-- Name: pk_elmah_error; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY elmah_error
    ADD CONSTRAINT pk_elmah_error PRIMARY KEY (errorid);


--
-- TOC entry 2025 (class 2606 OID 129465)
-- Name: profiledata_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_pkey PRIMARY KEY ("pId");


--
-- TOC entry 2027 (class 2606 OID 129467)
-- Name: profiledata_profile_name_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_name_unique UNIQUE ("Profile", "Name");


--
-- TOC entry 2030 (class 2606 OID 129469)
-- Name: profiles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_pkey PRIMARY KEY ("pId");


--
-- TOC entry 2032 (class 2606 OID 129471)
-- Name: profiles_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 2034 (class 2606 OID 129475)
-- Name: roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Roles"
    ADD CONSTRAINT roles_pkey PRIMARY KEY ("Rolename", "ApplicationName");


--
-- TOC entry 2036 (class 2606 OID 129477)
-- Name: sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Sessions"
    ADD CONSTRAINT sessions_pkey PRIMARY KEY ("SessionId", "ApplicationName");


--
-- TOC entry 2091 (class 2606 OID 129483)
-- Name: task_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT task_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2095 (class 2606 OID 129485)
-- Name: task_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT task_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2089 (class 2606 OID 129487)
-- Name: task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task
    ADD CONSTRAINT task_pkey PRIMARY KEY (id);


--
-- TOC entry 2097 (class 2606 OID 129493)
-- Name: task_template_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_template
    ADD CONSTRAINT task_template_pkey PRIMARY KEY (id);


--
-- TOC entry 2099 (class 2606 OID 129495)
-- Name: task_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT task_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2101 (class 2606 OID 129497)
-- Name: time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT time_pkey PRIMARY KEY (id);


--
-- TOC entry 2042 (class 2606 OID 129499)
-- Name: users_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT users_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 2044 (class 2606 OID 129501)
-- Name: usersinroles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_pkey PRIMARY KEY ("Username", "Rolename", "ApplicationName");


--
-- TOC entry 2053 (class 1259 OID 129504)
-- Name: ix_elmah_error_app_time_seq; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX ix_elmah_error_app_time_seq ON elmah_error USING btree (application, timeutc DESC, sequence DESC);


--
-- TOC entry 2028 (class 1259 OID 129505)
-- Name: profiles_isanonymous_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX profiles_isanonymous_index ON "Profiles" USING btree ("IsAnonymous");


--
-- TOC entry 2039 (class 1259 OID 129507)
-- Name: users_email_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX users_email_index ON "Users" USING btree ("Email");


--
-- TOC entry 2040 (class 1259 OID 129508)
-- Name: users_islockedout_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX users_islockedout_index ON "Users" USING btree ("IsLockedOut");


--
-- TOC entry 2115 (class 2606 OID 129509)
-- Name: FK_billing_group_contact_BillToContactId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_group
    ADD CONSTRAINT "FK_billing_group_contact_BillToContactId_Id" FOREIGN KEY (bill_to_contact_id) REFERENCES contact(id);


--
-- TOC entry 2116 (class 2606 OID 129514)
-- Name: FK_contact_billing_rate_BillingRateId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_billing_rate_BillingRateId" FOREIGN KEY (billing_rate_id) REFERENCES billing_rate(id);


--
-- TOC entry 2112 (class 2606 OID 129519)
-- Name: FK_core_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_CreatedByUserId" FOREIGN KEY (created_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2113 (class 2606 OID 129524)
-- Name: FK_core_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2114 (class 2606 OID 129529)
-- Name: FK_core_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2117 (class 2606 OID 129614)
-- Name: FK_expense_matter_ExpenseId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT "FK_expense_matter_ExpenseId" FOREIGN KEY (expense_id) REFERENCES expense(id);


--
-- TOC entry 2118 (class 2606 OID 129619)
-- Name: FK_expense_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT "FK_expense_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2119 (class 2606 OID 129624)
-- Name: FK_external_session_users_UserPId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY external_session
    ADD CONSTRAINT "FK_external_session_users_UserPId" FOREIGN KEY (user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2122 (class 2606 OID 129629)
-- Name: FK_invoice_billing_group_BillingGroupIp; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "FK_invoice_billing_group_BillingGroupIp" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2123 (class 2606 OID 129634)
-- Name: FK_invoice_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "FK_invoice_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2131 (class 2606 OID 129639)
-- Name: FK_matter_billing_group_BillingGroupId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_group_BillingGroupId" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2132 (class 2606 OID 129644)
-- Name: FK_matter_billing_group_BillingGroupId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_group_BillingGroupId_Id" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2133 (class 2606 OID 129649)
-- Name: FK_matter_billing_rate_DefaultBillingRateId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_rate_DefaultBillingRateId_Id" FOREIGN KEY (default_billing_rate_id) REFERENCES billing_rate(id);


--
-- TOC entry 2141 (class 2606 OID 133982)
-- Name: FK_matter_contact_contact_AttorneyForContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_contact_AttorneyForContactId" FOREIGN KEY (attorney_for_contact_id) REFERENCES contact(id);


--
-- TOC entry 2142 (class 2606 OID 133987)
-- Name: FK_matter_contact_contact_SupportStaffForContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_contact_SupportStaffForContactId" FOREIGN KEY (support_staff_for_contact_id) REFERENCES contact(id);


--
-- TOC entry 2143 (class 2606 OID 133992)
-- Name: FK_matter_contact_contact_ThirdPartyPayorForContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_contact_ThirdPartyPayorForContactId" FOREIGN KEY (third_party_payor_for_contact_id) REFERENCES contact(id);


--
-- TOC entry 2139 (class 2606 OID 129659)
-- Name: FK_matter_contact_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2140 (class 2606 OID 129664)
-- Name: FK_matter_contact_user_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2137 (class 2606 OID 134050)
-- Name: FK_matter_court_geographical_jurisdiction_CourtGeographicalJuri; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_court_geographical_jurisdiction_CourtGeographicalJuri" FOREIGN KEY (court_geographical_jurisdiction_id) REFERENCES court_geographical_jurisdiction(id);


--
-- TOC entry 2138 (class 2606 OID 134055)
-- Name: FK_matter_court_sitting_in_city_CourtSittingInCityId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_court_sitting_in_city_CourtSittingInCityId" FOREIGN KEY (court_sitting_in_city_id) REFERENCES court_sitting_in_city(id);


--
-- TOC entry 2136 (class 2606 OID 134045)
-- Name: FK_matter_court_type_CourtTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_court_type_CourtTypeId" FOREIGN KEY (court_type_id) REFERENCES court_type(id);


--
-- TOC entry 2134 (class 2606 OID 129669)
-- Name: FK_matter_matter_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_ParentId" FOREIGN KEY (parent_id) REFERENCES matter(id);


--
-- TOC entry 2135 (class 2606 OID 129674)
-- Name: FK_matter_matter_type_MatterTypeId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_type_MatterTypeId_Id" FOREIGN KEY (matter_type_id) REFERENCES matter_type(id);


--
-- TOC entry 2144 (class 2606 OID 129689)
-- Name: FK_note_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2145 (class 2606 OID 129694)
-- Name: FK_note_matter_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2148 (class 2606 OID 129699)
-- Name: FK_note_task_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2149 (class 2606 OID 129704)
-- Name: FK_note_task_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2152 (class 2606 OID 129724)
-- Name: FK_task_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2153 (class 2606 OID 129729)
-- Name: FK_task_assigned_contact_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2154 (class 2606 OID 129734)
-- Name: FK_task_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2155 (class 2606 OID 129739)
-- Name: FK_task_matter_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2150 (class 2606 OID 129764)
-- Name: FK_task_task_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_ParentId" FOREIGN KEY (parent_id) REFERENCES task(id);


--
-- TOC entry 2151 (class 2606 OID 129769)
-- Name: FK_task_task_SequentialPredecessorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_SequentialPredecessorId" FOREIGN KEY (sequential_predecessor_id) REFERENCES task(id);


--
-- TOC entry 2156 (class 2606 OID 129774)
-- Name: FK_task_time_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2157 (class 2606 OID 129779)
-- Name: FK_task_time_user_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2158 (class 2606 OID 129784)
-- Name: FK_time_user_WorkerContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_WorkerContactId" FOREIGN KEY (worker_contact_id) REFERENCES contact(id);


--
-- TOC entry 2120 (class 2606 OID 129789)
-- Name: fee_matter_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT "fee_matter_FeeId" FOREIGN KEY (fee_id) REFERENCES fee(id);


--
-- TOC entry 2121 (class 2606 OID 129794)
-- Name: fee_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT "fee_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2124 (class 2606 OID 129814)
-- Name: invoice_contact_BillToContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "invoice_contact_BillToContactId" FOREIGN KEY (bill_to_contact_id) REFERENCES contact(id);


--
-- TOC entry 2125 (class 2606 OID 129819)
-- Name: invoice_expense_expense_ExpenseId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT "invoice_expense_expense_ExpenseId" FOREIGN KEY (expense_id) REFERENCES expense(id);


--
-- TOC entry 2126 (class 2606 OID 129824)
-- Name: invoice_expense_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT "invoice_expense_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2127 (class 2606 OID 129829)
-- Name: invoice_fee_fee_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT "invoice_fee_fee_FeeId" FOREIGN KEY (fee_id) REFERENCES fee(id);


--
-- TOC entry 2128 (class 2606 OID 129834)
-- Name: invoice_fee_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT "invoice_fee_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2129 (class 2606 OID 129839)
-- Name: invoice_time_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT "invoice_time_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2130 (class 2606 OID 129844)
-- Name: invoice_time_time_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT "invoice_time_time_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2146 (class 2606 OID 129849)
-- Name: note_notification_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT "note_notification_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2147 (class 2606 OID 129854)
-- Name: note_notification_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT "note_notification_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2108 (class 2606 OID 129859)
-- Name: profiledata_profile_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_fkey FOREIGN KEY ("Profile") REFERENCES "Profiles"("pId") ON DELETE CASCADE;


--
-- TOC entry 2109 (class 2606 OID 129864)
-- Name: profiles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2110 (class 2606 OID 129869)
-- Name: usersinroles_rolename_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_rolename_fkey FOREIGN KEY ("Rolename", "ApplicationName") REFERENCES "Roles"("Rolename", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2111 (class 2606 OID 129874)
-- Name: usersinroles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2272 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2016-02-21 14:28:58

--
-- PostgreSQL database dump complete
--

