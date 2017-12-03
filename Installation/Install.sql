--
-- PostgreSQL database dump
--

-- Dumped from database version 9.3.4
-- Dumped by pg_dump version 9.3.5
-- Started on 2017-12-02 20:15:51

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 261 (class 3079 OID 11787)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2634 (class 0 OID 0)
-- Dependencies: 261
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 170 (class 1259 OID 204803)
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
-- TOC entry 171 (class 1259 OID 204809)
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
-- TOC entry 172 (class 1259 OID 204815)
-- Name: Roles; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "Roles" (
    "Rolename" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL
);


ALTER TABLE public."Roles" OWNER TO postgres;

--
-- TOC entry 173 (class 1259 OID 204821)
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
-- TOC entry 174 (class 1259 OID 204827)
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
-- TOC entry 175 (class 1259 OID 204833)
-- Name: UsersInRoles; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "UsersInRoles" (
    "Username" character varying(255) NOT NULL,
    "Rolename" character varying(255) NOT NULL,
    "ApplicationName" character varying(255) NOT NULL
);


ALTER TABLE public."UsersInRoles" OWNER TO postgres;

--
-- TOC entry 176 (class 1259 OID 204839)
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
-- TOC entry 249 (class 1259 OID 209233)
-- Name: activity_base; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_base (
    id bigint NOT NULL,
    type integer NOT NULL,
    is_campaign_response boolean NOT NULL,
    subject text,
    body text,
    owner integer NOT NULL,
    priority integer NOT NULL,
    due timestamp without time zone,
    state boolean NOT NULL,
    status_reason integer NOT NULL,
    duration integer
)
INHERITS (core);


ALTER TABLE public.activity_base OWNER TO postgres;

--
-- TOC entry 248 (class 1259 OID 209231)
-- Name: activity_base_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE activity_base_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.activity_base_id_seq OWNER TO postgres;

--
-- TOC entry 2635 (class 0 OID 0)
-- Dependencies: 248
-- Name: activity_base_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE activity_base_id_seq OWNED BY activity_base.id;


--
-- TOC entry 250 (class 1259 OID 209262)
-- Name: activity_correspondence_base; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_correspondence_base (
    sender integer,
    recipient integer,
    direction integer
)
INHERITS (activity_base);


ALTER TABLE public.activity_correspondence_base OWNER TO postgres;

--
-- TOC entry 247 (class 1259 OID 209221)
-- Name: activity_direction; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_direction (
    id integer NOT NULL,
    title text NOT NULL,
    "order" integer NOT NULL
);


ALTER TABLE public.activity_direction OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 209306)
-- Name: activity_email; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_email (
    address text
)
INHERITS (activity_correspondence_base);


ALTER TABLE public.activity_email OWNER TO postgres;

--
-- TOC entry 252 (class 1259 OID 209350)
-- Name: activity_letter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_letter (
    address text
)
INHERITS (activity_correspondence_base);


ALTER TABLE public.activity_letter OWNER TO postgres;

--
-- TOC entry 253 (class 1259 OID 209394)
-- Name: activity_phonecall; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_phonecall (
    phone_number text
)
INHERITS (activity_correspondence_base);


ALTER TABLE public.activity_phonecall OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 209211)
-- Name: activity_priority; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_priority (
    id integer NOT NULL,
    title text NOT NULL,
    "order" integer NOT NULL,
    "default" boolean NOT NULL
);


ALTER TABLE public.activity_priority OWNER TO postgres;

--
-- TOC entry 256 (class 1259 OID 209469)
-- Name: activity_regarding_base; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_regarding_base (
    id bigint NOT NULL,
    type integer NOT NULL,
    activity bigint NOT NULL
);


ALTER TABLE public.activity_regarding_base OWNER TO postgres;

--
-- TOC entry 255 (class 1259 OID 209467)
-- Name: activity_regarding_base_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE activity_regarding_base_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.activity_regarding_base_id_seq OWNER TO postgres;

--
-- TOC entry 2636 (class 0 OID 0)
-- Dependencies: 255
-- Name: activity_regarding_base_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE activity_regarding_base_id_seq OWNED BY activity_regarding_base.id;


--
-- TOC entry 257 (class 1259 OID 209485)
-- Name: activity_regarding_lead; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_regarding_lead (
    lead bigint NOT NULL
)
INHERITS (activity_regarding_base);


ALTER TABLE public.activity_regarding_lead OWNER TO postgres;

--
-- TOC entry 258 (class 1259 OID 209496)
-- Name: activity_regarding_opportunity; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_regarding_opportunity (
    opportunity bigint NOT NULL
)
INHERITS (activity_regarding_base);


ALTER TABLE public.activity_regarding_opportunity OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 209201)
-- Name: activity_regarding_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_regarding_type (
    id integer NOT NULL,
    title text NOT NULL,
    "order" integer NOT NULL,
    "default" boolean NOT NULL
);


ALTER TABLE public.activity_regarding_type OWNER TO postgres;

--
-- TOC entry 244 (class 1259 OID 209190)
-- Name: activity_status_reason; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_status_reason (
    id integer NOT NULL,
    "primary" text NOT NULL,
    secondary text NOT NULL,
    "order" integer NOT NULL
);


ALTER TABLE public.activity_status_reason OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 209188)
-- Name: activity_status_reason_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE activity_status_reason_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.activity_status_reason_id_seq OWNER TO postgres;

--
-- TOC entry 2637 (class 0 OID 0)
-- Dependencies: 243
-- Name: activity_status_reason_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE activity_status_reason_id_seq OWNED BY activity_status_reason.id;


--
-- TOC entry 254 (class 1259 OID 209438)
-- Name: activity_task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_task (
)
INHERITS (activity_base);


ALTER TABLE public.activity_task OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 209178)
-- Name: activity_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE activity_type (
    id integer NOT NULL,
    title text NOT NULL,
    "order" integer NOT NULL
);


ALTER TABLE public.activity_type OWNER TO postgres;

--
-- TOC entry 177 (class 1259 OID 204842)
-- Name: asset; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE asset (
    id uuid NOT NULL,
    id_int bigint NOT NULL,
    date timestamp without time zone NOT NULL,
    title text NOT NULL,
    is_final boolean NOT NULL,
    is_court_filed boolean NOT NULL,
    is_attorney_work_product boolean NOT NULL,
    is_privileged boolean NOT NULL,
    is_discoverable boolean NOT NULL,
    provided_in_discovery_at timestamp without time zone,
    checked_out_by_id uuid,
    checked_out_at timestamp without time zone
)
INHERITS (core);


ALTER TABLE public.asset OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 204848)
-- Name: asset_asset_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE asset_asset_tag (
    id uuid NOT NULL,
    asset_id uuid NOT NULL,
    asset_tag_id integer NOT NULL
)
INHERITS (core);


ALTER TABLE public.asset_asset_tag OWNER TO postgres;

--
-- TOC entry 179 (class 1259 OID 204851)
-- Name: asset_id_int_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE asset_id_int_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.asset_id_int_seq OWNER TO postgres;

--
-- TOC entry 2638 (class 0 OID 0)
-- Dependencies: 179
-- Name: asset_id_int_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE asset_id_int_seq OWNED BY asset.id_int;


--
-- TOC entry 180 (class 1259 OID 204853)
-- Name: asset_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE asset_matter (
    id uuid NOT NULL,
    asset_id uuid NOT NULL,
    matter_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.asset_matter OWNER TO postgres;

--
-- TOC entry 181 (class 1259 OID 204856)
-- Name: asset_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE asset_tag (
    id integer NOT NULL,
    name text NOT NULL
)
INHERITS (core);


ALTER TABLE public.asset_tag OWNER TO postgres;

--
-- TOC entry 182 (class 1259 OID 204862)
-- Name: asset_tag_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE asset_tag_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.asset_tag_id_seq OWNER TO postgres;

--
-- TOC entry 2639 (class 0 OID 0)
-- Dependencies: 182
-- Name: asset_tag_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE asset_tag_id_seq OWNED BY asset_tag.id;


--
-- TOC entry 183 (class 1259 OID 204864)
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
-- TOC entry 184 (class 1259 OID 204870)
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
-- TOC entry 2640 (class 0 OID 0)
-- Dependencies: 184
-- Name: billing_group_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE billing_group_id_seq OWNED BY billing_group.id;


--
-- TOC entry 185 (class 1259 OID 204872)
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
-- TOC entry 186 (class 1259 OID 204878)
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
-- TOC entry 2641 (class 0 OID 0)
-- Dependencies: 186
-- Name: billing_rate_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE billing_rate_id_seq OWNED BY billing_rate.id;


--
-- TOC entry 187 (class 1259 OID 204880)
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
-- TOC entry 188 (class 1259 OID 204886)
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
-- TOC entry 2642 (class 0 OID 0)
-- Dependencies: 188
-- Name: contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE contact_id_seq OWNED BY contact.id;


--
-- TOC entry 189 (class 1259 OID 204888)
-- Name: court_geographical_jurisdiction; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE court_geographical_jurisdiction (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.court_geographical_jurisdiction OWNER TO postgres;

--
-- TOC entry 190 (class 1259 OID 204894)
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
-- TOC entry 2643 (class 0 OID 0)
-- Dependencies: 190
-- Name: court_geographical_jurisdiction_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE court_geographical_jurisdiction_id_seq OWNED BY court_geographical_jurisdiction.id;


--
-- TOC entry 191 (class 1259 OID 204896)
-- Name: court_sitting_in_city; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE court_sitting_in_city (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.court_sitting_in_city OWNER TO postgres;

--
-- TOC entry 192 (class 1259 OID 204902)
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
-- TOC entry 2644 (class 0 OID 0)
-- Dependencies: 192
-- Name: court_sitting_in_city_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE court_sitting_in_city_id_seq OWNED BY court_sitting_in_city.id;


--
-- TOC entry 193 (class 1259 OID 204904)
-- Name: court_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE court_type (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.court_type OWNER TO postgres;

--
-- TOC entry 194 (class 1259 OID 204910)
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
-- TOC entry 2645 (class 0 OID 0)
-- Dependencies: 194
-- Name: court_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE court_type_id_seq OWNED BY court_type.id;


--
-- TOC entry 195 (class 1259 OID 204912)
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
-- TOC entry 196 (class 1259 OID 204914)
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
-- TOC entry 197 (class 1259 OID 204921)
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
-- TOC entry 198 (class 1259 OID 204927)
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
-- TOC entry 199 (class 1259 OID 204930)
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
-- TOC entry 200 (class 1259 OID 204936)
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
-- TOC entry 201 (class 1259 OID 204942)
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
-- TOC entry 202 (class 1259 OID 204945)
-- Name: file; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE file (
    id uuid NOT NULL,
    version_id uuid NOT NULL,
    is_source boolean NOT NULL,
    content_length bigint NOT NULL,
    content_type text NOT NULL,
    extension text
)
INHERITS (core);


ALTER TABLE public.file OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 204951)
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
-- TOC entry 204 (class 1259 OID 204957)
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
-- TOC entry 205 (class 1259 OID 204963)
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
-- TOC entry 206 (class 1259 OID 204969)
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
-- TOC entry 237 (class 1259 OID 209102)
-- Name: lead; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE lead (
    id bigint NOT NULL,
    status_id integer NOT NULL,
    contact_id integer NOT NULL,
    source_id integer,
    fee_id integer,
    closed timestamp without time zone,
    details text
)
INHERITS (core);


ALTER TABLE public.lead OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 209086)
-- Name: lead_fee; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE lead_fee (
    id integer NOT NULL,
    is_eligible boolean NOT NULL,
    amount money,
    to_id integer,
    paid timestamp with time zone,
    additional_data text
)
INHERITS (core);


ALTER TABLE public.lead_fee OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 209084)
-- Name: lead_fee_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE lead_fee_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.lead_fee_id_seq OWNER TO postgres;

--
-- TOC entry 2646 (class 0 OID 0)
-- Dependencies: 234
-- Name: lead_fee_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE lead_fee_id_seq OWNED BY lead_fee.id;


--
-- TOC entry 236 (class 1259 OID 209100)
-- Name: lead_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE lead_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.lead_id_seq OWNER TO postgres;

--
-- TOC entry 2647 (class 0 OID 0)
-- Dependencies: 236
-- Name: lead_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE lead_id_seq OWNED BY lead.id;


--
-- TOC entry 233 (class 1259 OID 209065)
-- Name: lead_source; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE lead_source (
    id integer NOT NULL,
    type_id integer NOT NULL,
    contact_id integer,
    title text NOT NULL,
    additional_question_1 text,
    additional_data_1 text,
    additional_question_2 text,
    additional_data_2 text
)
INHERITS (core);


ALTER TABLE public.lead_source OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 209063)
-- Name: lead_source_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE lead_source_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.lead_source_id_seq OWNER TO postgres;

--
-- TOC entry 2648 (class 0 OID 0)
-- Dependencies: 232
-- Name: lead_source_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE lead_source_id_seq OWNED BY lead_source.id;


--
-- TOC entry 231 (class 1259 OID 209054)
-- Name: lead_source_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE lead_source_type (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.lead_source_type OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 209052)
-- Name: lead_source_type_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE lead_source_type_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.lead_source_type_id_seq OWNER TO postgres;

--
-- TOC entry 2649 (class 0 OID 0)
-- Dependencies: 230
-- Name: lead_source_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE lead_source_type_id_seq OWNED BY lead_source_type.id;


--
-- TOC entry 229 (class 1259 OID 209043)
-- Name: lead_status; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE lead_status (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.lead_status OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 209041)
-- Name: lead_status_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE lead_status_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.lead_status_id_seq OWNER TO postgres;

--
-- TOC entry 2650 (class 0 OID 0)
-- Dependencies: 228
-- Name: lead_status_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE lead_status_id_seq OWNED BY lead_status.id;


--
-- TOC entry 207 (class 1259 OID 204975)
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
    caption_other_party_long text,
    id_int bigint NOT NULL
)
INHERITS (core);


ALTER TABLE public.matter OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 204982)
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
-- TOC entry 209 (class 1259 OID 204988)
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
-- TOC entry 2651 (class 0 OID 0)
-- Dependencies: 209
-- Name: matter_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_contact_id_seq OWNED BY matter_contact.id;


--
-- TOC entry 210 (class 1259 OID 204990)
-- Name: matter_id_int_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE matter_id_int_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.matter_id_int_seq OWNER TO postgres;

--
-- TOC entry 2652 (class 0 OID 0)
-- Dependencies: 210
-- Name: matter_id_int_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_id_int_seq OWNED BY matter.id_int;


--
-- TOC entry 211 (class 1259 OID 204992)
-- Name: matter_type; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_type (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.matter_type OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 204998)
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
-- TOC entry 2653 (class 0 OID 0)
-- Dependencies: 212
-- Name: matter_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_type_id_seq OWNED BY matter_type.id;


--
-- TOC entry 213 (class 1259 OID 205000)
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
-- TOC entry 214 (class 1259 OID 205006)
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
-- TOC entry 215 (class 1259 OID 205009)
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
-- TOC entry 216 (class 1259 OID 205012)
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
-- TOC entry 241 (class 1259 OID 209146)
-- Name: opportunity; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE opportunity (
    id bigint NOT NULL,
    account_id integer NOT NULL,
    stage_id integer NOT NULL,
    probability numeric(4,4),
    amount money,
    closed timestamp with time zone,
    lead_id integer NOT NULL,
    matter_id uuid
)
INHERITS (core);


ALTER TABLE public.opportunity OWNER TO postgres;

--
-- TOC entry 260 (class 1259 OID 209510)
-- Name: opportunity_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE opportunity_contact (
    id integer NOT NULL,
    contact_id integer NOT NULL,
    opportunity_id bigint NOT NULL
)
INHERITS (core);


ALTER TABLE public.opportunity_contact OWNER TO postgres;

--
-- TOC entry 259 (class 1259 OID 209508)
-- Name: opportunity_contact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE opportunity_contact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.opportunity_contact_id_seq OWNER TO postgres;

--
-- TOC entry 2654 (class 0 OID 0)
-- Dependencies: 259
-- Name: opportunity_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE opportunity_contact_id_seq OWNED BY opportunity_contact.id;


--
-- TOC entry 240 (class 1259 OID 209144)
-- Name: opportunity_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE opportunity_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.opportunity_id_seq OWNER TO postgres;

--
-- TOC entry 2655 (class 0 OID 0)
-- Dependencies: 240
-- Name: opportunity_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE opportunity_id_seq OWNED BY opportunity.id;


--
-- TOC entry 239 (class 1259 OID 209133)
-- Name: opportunity_stage; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE opportunity_stage (
    id integer NOT NULL,
    "order" integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.opportunity_stage OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 209131)
-- Name: opportunity_stage_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE opportunity_stage_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.opportunity_stage_id_seq OWNER TO postgres;

--
-- TOC entry 2656 (class 0 OID 0)
-- Dependencies: 238
-- Name: opportunity_stage_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE opportunity_stage_id_seq OWNED BY opportunity_stage.id;


--
-- TOC entry 217 (class 1259 OID 205015)
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
-- TOC entry 218 (class 1259 OID 205021)
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
-- TOC entry 219 (class 1259 OID 205025)
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
-- TOC entry 2657 (class 0 OID 0)
-- Dependencies: 219
-- Name: task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_id_seq OWNED BY task.id;


--
-- TOC entry 220 (class 1259 OID 205027)
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
-- TOC entry 221 (class 1259 OID 205030)
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
-- TOC entry 222 (class 1259 OID 205036)
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
-- TOC entry 2658 (class 0 OID 0)
-- Dependencies: 222
-- Name: task_template_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_template_id_seq OWNED BY task_template.id;


--
-- TOC entry 223 (class 1259 OID 205038)
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
-- TOC entry 224 (class 1259 OID 205041)
-- Name: time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "time" (
    id uuid NOT NULL,
    start timestamp without time zone NOT NULL,
    stop timestamp without time zone,
    worker_contact_id integer NOT NULL,
    details text,
    billable boolean DEFAULT false NOT NULL,
    time_category_id integer
)
INHERITS (core);


ALTER TABLE public."time" OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 205048)
-- Name: time_category; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE time_category (
    id integer NOT NULL,
    title text NOT NULL
)
INHERITS (core);


ALTER TABLE public.time_category OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 205054)
-- Name: time_category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE time_category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.time_category_id_seq OWNER TO postgres;

--
-- TOC entry 2659 (class 0 OID 0)
-- Dependencies: 226
-- Name: time_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE time_category_id_seq OWNED BY time_category.id;


--
-- TOC entry 227 (class 1259 OID 205056)
-- Name: version; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE version (
    id uuid NOT NULL,
    sequence_number integer NOT NULL,
    change_details text,
    asset_id uuid NOT NULL
)
INHERITS (core);


ALTER TABLE public.version OWNER TO postgres;

--
-- TOC entry 2240 (class 2604 OID 209236)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_base ALTER COLUMN id SET DEFAULT nextval('activity_base_id_seq'::regclass);


--
-- TOC entry 2241 (class 2604 OID 209265)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base ALTER COLUMN id SET DEFAULT nextval('activity_base_id_seq'::regclass);


--
-- TOC entry 2242 (class 2604 OID 209309)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email ALTER COLUMN id SET DEFAULT nextval('activity_base_id_seq'::regclass);


--
-- TOC entry 2243 (class 2604 OID 209353)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter ALTER COLUMN id SET DEFAULT nextval('activity_base_id_seq'::regclass);


--
-- TOC entry 2244 (class 2604 OID 209397)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall ALTER COLUMN id SET DEFAULT nextval('activity_base_id_seq'::regclass);


--
-- TOC entry 2246 (class 2604 OID 209472)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_regarding_base ALTER COLUMN id SET DEFAULT nextval('activity_regarding_base_id_seq'::regclass);


--
-- TOC entry 2247 (class 2604 OID 209488)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_regarding_lead ALTER COLUMN id SET DEFAULT nextval('activity_regarding_base_id_seq'::regclass);


--
-- TOC entry 2248 (class 2604 OID 209499)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_regarding_opportunity ALTER COLUMN id SET DEFAULT nextval('activity_regarding_base_id_seq'::regclass);


--
-- TOC entry 2239 (class 2604 OID 209193)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_status_reason ALTER COLUMN id SET DEFAULT nextval('activity_status_reason_id_seq'::regclass);


--
-- TOC entry 2245 (class 2604 OID 209441)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_task ALTER COLUMN id SET DEFAULT nextval('activity_base_id_seq'::regclass);


--
-- TOC entry 2214 (class 2604 OID 205062)
-- Name: id_int; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY asset ALTER COLUMN id_int SET DEFAULT nextval('asset_id_int_seq'::regclass);


--
-- TOC entry 2215 (class 2604 OID 205063)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY asset_tag ALTER COLUMN id SET DEFAULT nextval('asset_tag_id_seq'::regclass);


--
-- TOC entry 2216 (class 2604 OID 205064)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_group ALTER COLUMN id SET DEFAULT nextval('billing_group_id_seq'::regclass);


--
-- TOC entry 2217 (class 2604 OID 205065)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_rate ALTER COLUMN id SET DEFAULT nextval('billing_rate_id_seq'::regclass);


--
-- TOC entry 2218 (class 2604 OID 205066)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact ALTER COLUMN id SET DEFAULT nextval('contact_id_seq'::regclass);


--
-- TOC entry 2219 (class 2604 OID 205067)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY court_geographical_jurisdiction ALTER COLUMN id SET DEFAULT nextval('court_geographical_jurisdiction_id_seq'::regclass);


--
-- TOC entry 2220 (class 2604 OID 205068)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY court_sitting_in_city ALTER COLUMN id SET DEFAULT nextval('court_sitting_in_city_id_seq'::regclass);


--
-- TOC entry 2221 (class 2604 OID 205069)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY court_type ALTER COLUMN id SET DEFAULT nextval('court_type_id_seq'::regclass);


--
-- TOC entry 2236 (class 2604 OID 209105)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead ALTER COLUMN id SET DEFAULT nextval('lead_id_seq'::regclass);


--
-- TOC entry 2235 (class 2604 OID 209089)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead_fee ALTER COLUMN id SET DEFAULT nextval('lead_fee_id_seq'::regclass);


--
-- TOC entry 2234 (class 2604 OID 209068)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead_source ALTER COLUMN id SET DEFAULT nextval('lead_source_id_seq'::regclass);


--
-- TOC entry 2233 (class 2604 OID 209057)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead_source_type ALTER COLUMN id SET DEFAULT nextval('lead_source_type_id_seq'::regclass);


--
-- TOC entry 2232 (class 2604 OID 209046)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead_status ALTER COLUMN id SET DEFAULT nextval('lead_status_id_seq'::regclass);


--
-- TOC entry 2224 (class 2604 OID 205070)
-- Name: id_int; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter ALTER COLUMN id_int SET DEFAULT nextval('matter_id_int_seq'::regclass);


--
-- TOC entry 2225 (class 2604 OID 205071)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact ALTER COLUMN id SET DEFAULT nextval('matter_contact_id_seq'::regclass);


--
-- TOC entry 2226 (class 2604 OID 205072)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_type ALTER COLUMN id SET DEFAULT nextval('matter_type_id_seq'::regclass);


--
-- TOC entry 2238 (class 2604 OID 209149)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity ALTER COLUMN id SET DEFAULT nextval('opportunity_id_seq'::regclass);


--
-- TOC entry 2249 (class 2604 OID 209513)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity_contact ALTER COLUMN id SET DEFAULT nextval('opportunity_contact_id_seq'::regclass);


--
-- TOC entry 2237 (class 2604 OID 209136)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity_stage ALTER COLUMN id SET DEFAULT nextval('opportunity_stage_id_seq'::regclass);


--
-- TOC entry 2227 (class 2604 OID 205073)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task ALTER COLUMN id SET DEFAULT nextval('task_id_seq'::regclass);


--
-- TOC entry 2229 (class 2604 OID 205074)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_template ALTER COLUMN id SET DEFAULT nextval('task_template_id_seq'::regclass);


--
-- TOC entry 2231 (class 2604 OID 205075)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY time_category ALTER COLUMN id SET DEFAULT nextval('time_category_id_seq'::regclass);


--
-- TOC entry 2337 (class 2606 OID 208543)
-- Name: UQ_task_matter_Task_Matter; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "UQ_task_matter_Task_Matter" UNIQUE (task_id, matter_id);


--
-- TOC entry 2264 (class 2606 OID 208545)
-- Name: Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("pId");


--
-- TOC entry 2389 (class 2606 OID 209241)
-- Name: activity_base_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_base
    ADD CONSTRAINT activity_base_pkey PRIMARY KEY (id);


--
-- TOC entry 2391 (class 2606 OID 209270)
-- Name: activity_correspondence_base_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_pkey PRIMARY KEY (id);


--
-- TOC entry 2385 (class 2606 OID 209230)
-- Name: activity_direction_order_UK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_direction
    ADD CONSTRAINT "activity_direction_order_UK" UNIQUE ("order");


--
-- TOC entry 2387 (class 2606 OID 209228)
-- Name: activity_direction_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_direction
    ADD CONSTRAINT activity_direction_pkey PRIMARY KEY (id);


--
-- TOC entry 2393 (class 2606 OID 209314)
-- Name: activity_email_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_pkey PRIMARY KEY (id);


--
-- TOC entry 2395 (class 2606 OID 209358)
-- Name: activity_letter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_pkey PRIMARY KEY (id);


--
-- TOC entry 2397 (class 2606 OID 209402)
-- Name: activity_phonecall_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_pkey PRIMARY KEY (id);


--
-- TOC entry 2381 (class 2606 OID 209220)
-- Name: activity_priority_order_UK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_priority
    ADD CONSTRAINT "activity_priority_order_UK" UNIQUE ("order");


--
-- TOC entry 2383 (class 2606 OID 209218)
-- Name: activity_priority_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_priority
    ADD CONSTRAINT activity_priority_pkey PRIMARY KEY (id);


--
-- TOC entry 2401 (class 2606 OID 209474)
-- Name: activity_regarding_base_PKEY; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_regarding_base
    ADD CONSTRAINT "activity_regarding_base_PKEY" PRIMARY KEY (id);


--
-- TOC entry 2403 (class 2606 OID 209490)
-- Name: activity_regarding_lead_PKEY; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_regarding_lead
    ADD CONSTRAINT "activity_regarding_lead_PKEY" PRIMARY KEY (id);


--
-- TOC entry 2405 (class 2606 OID 209501)
-- Name: activity_regarding_opportunity_PKEY; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_regarding_opportunity
    ADD CONSTRAINT "activity_regarding_opportunity_PKEY" PRIMARY KEY (id);


--
-- TOC entry 2377 (class 2606 OID 209208)
-- Name: activity_regarding_type_PKEY; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_regarding_type
    ADD CONSTRAINT "activity_regarding_type_PKEY" PRIMARY KEY (id);


--
-- TOC entry 2379 (class 2606 OID 209210)
-- Name: activity_regarding_type_order_UK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_regarding_type
    ADD CONSTRAINT "activity_regarding_type_order_UK" UNIQUE ("order");


--
-- TOC entry 2373 (class 2606 OID 209200)
-- Name: activity_status_reason_order_UK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_status_reason
    ADD CONSTRAINT "activity_status_reason_order_UK" UNIQUE ("order");


--
-- TOC entry 2375 (class 2606 OID 209198)
-- Name: activity_status_reason_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_status_reason
    ADD CONSTRAINT activity_status_reason_pkey PRIMARY KEY (id);


--
-- TOC entry 2399 (class 2606 OID 209446)
-- Name: activity_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_task
    ADD CONSTRAINT activity_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2369 (class 2606 OID 209187)
-- Name: activity_type_order_UK; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_type
    ADD CONSTRAINT "activity_type_order_UK" UNIQUE ("order");


--
-- TOC entry 2371 (class 2606 OID 209185)
-- Name: activity_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY activity_type
    ADD CONSTRAINT activity_type_pkey PRIMARY KEY (id);


--
-- TOC entry 2274 (class 2606 OID 208547)
-- Name: asset_asset_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY asset_asset_tag
    ADD CONSTRAINT asset_asset_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2278 (class 2606 OID 208549)
-- Name: asset_tag_name_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY asset_tag
    ADD CONSTRAINT asset_tag_name_unique UNIQUE (name);


--
-- TOC entry 2280 (class 2606 OID 208551)
-- Name: asset_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY asset_tag
    ADD CONSTRAINT asset_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2272 (class 2606 OID 208553)
-- Name: assets_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY asset
    ADD CONSTRAINT assets_pkey PRIMARY KEY (id);


--
-- TOC entry 2282 (class 2606 OID 208555)
-- Name: billing_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_group
    ADD CONSTRAINT billing_group_pkey PRIMARY KEY (id);


--
-- TOC entry 2284 (class 2606 OID 208557)
-- Name: billing_rates_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_rate
    ADD CONSTRAINT billing_rates_pkey PRIMARY KEY (id);


--
-- TOC entry 2286 (class 2606 OID 208559)
-- Name: billing_rates_title_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY billing_rate
    ADD CONSTRAINT billing_rates_title_unique UNIQUE (title);


--
-- TOC entry 2288 (class 2606 OID 208561)
-- Name: contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2290 (class 2606 OID 208563)
-- Name: court_geographical_jurisdiction_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY court_geographical_jurisdiction
    ADD CONSTRAINT court_geographical_jurisdiction_pkey PRIMARY KEY (id);


--
-- TOC entry 2292 (class 2606 OID 208565)
-- Name: court_sitting_in_city_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY court_sitting_in_city
    ADD CONSTRAINT court_sitting_in_city_pkey PRIMARY KEY (id);


--
-- TOC entry 2294 (class 2606 OID 208567)
-- Name: court_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY court_type
    ADD CONSTRAINT court_type_pkey PRIMARY KEY (id);


--
-- TOC entry 2301 (class 2606 OID 208569)
-- Name: expense_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT expense_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2299 (class 2606 OID 208571)
-- Name: expense_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY expense
    ADD CONSTRAINT expense_pkey PRIMARY KEY (id);


--
-- TOC entry 2303 (class 2606 OID 208573)
-- Name: external_session_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY external_session
    ADD CONSTRAINT external_session_pkey PRIMARY KEY (id);


--
-- TOC entry 2307 (class 2606 OID 208575)
-- Name: fee_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT fee_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2305 (class 2606 OID 208577)
-- Name: fee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY fee
    ADD CONSTRAINT fee_pkey PRIMARY KEY (id);


--
-- TOC entry 2309 (class 2606 OID 208579)
-- Name: file_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY file
    ADD CONSTRAINT file_pkey PRIMARY KEY (id);


--
-- TOC entry 2313 (class 2606 OID 208581)
-- Name: invoice_expense_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT invoice_expense_pkey PRIMARY KEY (id);


--
-- TOC entry 2315 (class 2606 OID 208583)
-- Name: invoice_fee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT invoice_fee_pkey PRIMARY KEY (id);


--
-- TOC entry 2311 (class 2606 OID 208585)
-- Name: invoice_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT invoice_pkey PRIMARY KEY (id);


--
-- TOC entry 2317 (class 2606 OID 208587)
-- Name: invoice_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT invoice_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2361 (class 2606 OID 209110)
-- Name: lead_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY lead
    ADD CONSTRAINT lead_pkey PRIMARY KEY (id);


--
-- TOC entry 2357 (class 2606 OID 209073)
-- Name: lead_source_pk; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY lead_source
    ADD CONSTRAINT lead_source_pk PRIMARY KEY (id);


--
-- TOC entry 2355 (class 2606 OID 209062)
-- Name: lead_source_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY lead_source_type
    ADD CONSTRAINT lead_source_type_pkey PRIMARY KEY (id);


--
-- TOC entry 2353 (class 2606 OID 209051)
-- Name: lead_status_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY lead_status
    ADD CONSTRAINT lead_status_pkey PRIMARY KEY (id);


--
-- TOC entry 2359 (class 2606 OID 209094)
-- Name: load_fee_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY lead_fee
    ADD CONSTRAINT load_fee_pkey PRIMARY KEY (id);


--
-- TOC entry 2321 (class 2606 OID 208589)
-- Name: matter_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT matter_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2319 (class 2606 OID 208591)
-- Name: matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2323 (class 2606 OID 208593)
-- Name: matter_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_type
    ADD CONSTRAINT matter_type_pkey PRIMARY KEY (id);


--
-- TOC entry 2327 (class 2606 OID 208595)
-- Name: note_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT note_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2329 (class 2606 OID 208597)
-- Name: note_notification_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT note_notification_pkey PRIMARY KEY (id);


--
-- TOC entry 2325 (class 2606 OID 208599)
-- Name: note_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note
    ADD CONSTRAINT note_pkey PRIMARY KEY (id);


--
-- TOC entry 2331 (class 2606 OID 208601)
-- Name: note_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT note_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2407 (class 2606 OID 209515)
-- Name: opportunity_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY opportunity_contact
    ADD CONSTRAINT opportunity_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2367 (class 2606 OID 209151)
-- Name: opportunity_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY opportunity
    ADD CONSTRAINT opportunity_pkey PRIMARY KEY (id);


--
-- TOC entry 2363 (class 2606 OID 209143)
-- Name: opportunity_stage_order_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY opportunity_stage
    ADD CONSTRAINT opportunity_stage_order_unique UNIQUE ("order");


--
-- TOC entry 2365 (class 2606 OID 209141)
-- Name: opportunity_stage_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY opportunity_stage
    ADD CONSTRAINT opportunity_stage_pkey PRIMARY KEY (id);


--
-- TOC entry 2297 (class 2606 OID 208603)
-- Name: pk_elmah_error; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY elmah_error
    ADD CONSTRAINT pk_elmah_error PRIMARY KEY (errorid);


--
-- TOC entry 2276 (class 2606 OID 208605)
-- Name: pkey_asset_matter; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY asset_matter
    ADD CONSTRAINT pkey_asset_matter PRIMARY KEY (id);


--
-- TOC entry 2251 (class 2606 OID 208607)
-- Name: profiledata_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_pkey PRIMARY KEY ("pId");


--
-- TOC entry 2253 (class 2606 OID 208609)
-- Name: profiledata_profile_name_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_name_unique UNIQUE ("Profile", "Name");


--
-- TOC entry 2256 (class 2606 OID 208611)
-- Name: profiles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_pkey PRIMARY KEY ("pId");


--
-- TOC entry 2258 (class 2606 OID 208613)
-- Name: profiles_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 2260 (class 2606 OID 208615)
-- Name: roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Roles"
    ADD CONSTRAINT roles_pkey PRIMARY KEY ("Rolename", "ApplicationName");


--
-- TOC entry 2262 (class 2606 OID 208617)
-- Name: sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Sessions"
    ADD CONSTRAINT sessions_pkey PRIMARY KEY ("SessionId", "ApplicationName");


--
-- TOC entry 2335 (class 2606 OID 208619)
-- Name: task_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT task_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2339 (class 2606 OID 208621)
-- Name: task_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT task_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2333 (class 2606 OID 208623)
-- Name: task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task
    ADD CONSTRAINT task_pkey PRIMARY KEY (id);


--
-- TOC entry 2341 (class 2606 OID 208625)
-- Name: task_template_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_template
    ADD CONSTRAINT task_template_pkey PRIMARY KEY (id);


--
-- TOC entry 2343 (class 2606 OID 208627)
-- Name: task_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT task_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2347 (class 2606 OID 208629)
-- Name: time_category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY time_category
    ADD CONSTRAINT time_category_pkey PRIMARY KEY (id);


--
-- TOC entry 2349 (class 2606 OID 208631)
-- Name: time_category_title_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY time_category
    ADD CONSTRAINT time_category_title_unique UNIQUE (title);


--
-- TOC entry 2345 (class 2606 OID 208633)
-- Name: time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT time_pkey PRIMARY KEY (id);


--
-- TOC entry 2268 (class 2606 OID 208635)
-- Name: users_username_application_unique; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "Users"
    ADD CONSTRAINT users_username_application_unique UNIQUE ("Username", "ApplicationName");


--
-- TOC entry 2270 (class 2606 OID 208637)
-- Name: usersinroles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_pkey PRIMARY KEY ("Username", "Rolename", "ApplicationName");


--
-- TOC entry 2351 (class 2606 OID 208639)
-- Name: version_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY version
    ADD CONSTRAINT version_pkey PRIMARY KEY (id);


--
-- TOC entry 2295 (class 1259 OID 208640)
-- Name: ix_elmah_error_app_time_seq; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX ix_elmah_error_app_time_seq ON elmah_error USING btree (application, timeutc DESC, sequence DESC);


--
-- TOC entry 2254 (class 1259 OID 208641)
-- Name: profiles_isanonymous_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX profiles_isanonymous_index ON "Profiles" USING btree ("IsAnonymous");


--
-- TOC entry 2265 (class 1259 OID 208642)
-- Name: users_email_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX users_email_index ON "Users" USING btree ("Email");


--
-- TOC entry 2266 (class 1259 OID 208643)
-- Name: users_islockedout_index; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX users_islockedout_index ON "Users" USING btree ("IsLockedOut");


--
-- TOC entry 2416 (class 2606 OID 208644)
-- Name: FK_asset_asset_tag_asset_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY asset_asset_tag
    ADD CONSTRAINT "FK_asset_asset_tag_asset_id" FOREIGN KEY (asset_id) REFERENCES asset(id);


--
-- TOC entry 2417 (class 2606 OID 208649)
-- Name: FK_asset_asset_tag_asset_tag_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY asset_asset_tag
    ADD CONSTRAINT "FK_asset_asset_tag_asset_tag_id" FOREIGN KEY (asset_tag_id) REFERENCES asset_tag(id);


--
-- TOC entry 2415 (class 2606 OID 208654)
-- Name: FK_asset_checked_out_by_users_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY asset
    ADD CONSTRAINT "FK_asset_checked_out_by_users_id" FOREIGN KEY (checked_out_by_id) REFERENCES "Users"("pId");


--
-- TOC entry 2418 (class 2606 OID 208659)
-- Name: FK_asset_matter_asset_id_asset_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY asset_matter
    ADD CONSTRAINT "FK_asset_matter_asset_id_asset_id" FOREIGN KEY (asset_id) REFERENCES asset(id);


--
-- TOC entry 2419 (class 2606 OID 208664)
-- Name: FK_asset_matter_matter_id_matter_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY asset_matter
    ADD CONSTRAINT "FK_asset_matter_matter_id_matter_id" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2420 (class 2606 OID 208669)
-- Name: FK_billing_group_contact_BillToContactId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY billing_group
    ADD CONSTRAINT "FK_billing_group_contact_BillToContactId_Id" FOREIGN KEY (bill_to_contact_id) REFERENCES contact(id);


--
-- TOC entry 2421 (class 2606 OID 208674)
-- Name: FK_contact_billing_rate_BillingRateId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_billing_rate_BillingRateId" FOREIGN KEY (billing_rate_id) REFERENCES billing_rate(id);


--
-- TOC entry 2412 (class 2606 OID 208679)
-- Name: FK_core_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_CreatedByUserId" FOREIGN KEY (created_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2413 (class 2606 OID 208684)
-- Name: FK_core_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2414 (class 2606 OID 208689)
-- Name: FK_core_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY core
    ADD CONSTRAINT "FK_core_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2422 (class 2606 OID 208694)
-- Name: FK_expense_matter_ExpenseId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT "FK_expense_matter_ExpenseId" FOREIGN KEY (expense_id) REFERENCES expense(id);


--
-- TOC entry 2423 (class 2606 OID 208699)
-- Name: FK_expense_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY expense_matter
    ADD CONSTRAINT "FK_expense_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2424 (class 2606 OID 208704)
-- Name: FK_external_session_users_UserPId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY external_session
    ADD CONSTRAINT "FK_external_session_users_UserPId" FOREIGN KEY (user_pid) REFERENCES "Users"("pId");


--
-- TOC entry 2427 (class 2606 OID 208709)
-- Name: FK_file_version_id_version_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY file
    ADD CONSTRAINT "FK_file_version_id_version_id" FOREIGN KEY (version_id) REFERENCES version(id);


--
-- TOC entry 2428 (class 2606 OID 208714)
-- Name: FK_invoice_billing_group_BillingGroupIp; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "FK_invoice_billing_group_BillingGroupIp" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2429 (class 2606 OID 208719)
-- Name: FK_invoice_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "FK_invoice_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2437 (class 2606 OID 208724)
-- Name: FK_matter_billing_group_BillingGroupId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_group_BillingGroupId" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2438 (class 2606 OID 208729)
-- Name: FK_matter_billing_group_BillingGroupId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_group_BillingGroupId_Id" FOREIGN KEY (billing_group_id) REFERENCES billing_group(id);


--
-- TOC entry 2439 (class 2606 OID 208734)
-- Name: FK_matter_billing_rate_DefaultBillingRateId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_billing_rate_DefaultBillingRateId_Id" FOREIGN KEY (default_billing_rate_id) REFERENCES billing_rate(id);


--
-- TOC entry 2445 (class 2606 OID 208739)
-- Name: FK_matter_contact_contact_AttorneyForContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_contact_AttorneyForContactId" FOREIGN KEY (attorney_for_contact_id) REFERENCES contact(id);


--
-- TOC entry 2446 (class 2606 OID 208744)
-- Name: FK_matter_contact_contact_SupportStaffForContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_contact_SupportStaffForContactId" FOREIGN KEY (support_staff_for_contact_id) REFERENCES contact(id);


--
-- TOC entry 2447 (class 2606 OID 208749)
-- Name: FK_matter_contact_contact_ThirdPartyPayorForContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_contact_ThirdPartyPayorForContactId" FOREIGN KEY (third_party_payor_for_contact_id) REFERENCES contact(id);


--
-- TOC entry 2448 (class 2606 OID 208754)
-- Name: FK_matter_contact_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2449 (class 2606 OID 208759)
-- Name: FK_matter_contact_user_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2440 (class 2606 OID 208764)
-- Name: FK_matter_court_geographical_jurisdiction_CourtGeographicalJuri; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_court_geographical_jurisdiction_CourtGeographicalJuri" FOREIGN KEY (court_geographical_jurisdiction_id) REFERENCES court_geographical_jurisdiction(id);


--
-- TOC entry 2441 (class 2606 OID 208769)
-- Name: FK_matter_court_sitting_in_city_CourtSittingInCityId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_court_sitting_in_city_CourtSittingInCityId" FOREIGN KEY (court_sitting_in_city_id) REFERENCES court_sitting_in_city(id);


--
-- TOC entry 2442 (class 2606 OID 208774)
-- Name: FK_matter_court_type_CourtTypeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_court_type_CourtTypeId" FOREIGN KEY (court_type_id) REFERENCES court_type(id);


--
-- TOC entry 2443 (class 2606 OID 208779)
-- Name: FK_matter_matter_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_ParentId" FOREIGN KEY (parent_id) REFERENCES matter(id);


--
-- TOC entry 2444 (class 2606 OID 208784)
-- Name: FK_matter_matter_type_MatterTypeId_Id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_type_MatterTypeId_Id" FOREIGN KEY (matter_type_id) REFERENCES matter_type(id);


--
-- TOC entry 2450 (class 2606 OID 208789)
-- Name: FK_note_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2451 (class 2606 OID 208794)
-- Name: FK_note_matter_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2454 (class 2606 OID 208799)
-- Name: FK_note_task_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2455 (class 2606 OID 208804)
-- Name: FK_note_task_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2519 (class 2606 OID 209516)
-- Name: FK_opportunity_contact_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity_contact
    ADD CONSTRAINT "FK_opportunity_contact_contact_id" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2518 (class 2606 OID 209521)
-- Name: FK_opportunity_contact_opportunity_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity_contact
    ADD CONSTRAINT "FK_opportunity_contact_opportunity_id" FOREIGN KEY (opportunity_id) REFERENCES opportunity(id);


--
-- TOC entry 2458 (class 2606 OID 208809)
-- Name: FK_task_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2459 (class 2606 OID 208814)
-- Name: FK_task_assigned_contact_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2460 (class 2606 OID 208819)
-- Name: FK_task_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2461 (class 2606 OID 208824)
-- Name: FK_task_matter_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2456 (class 2606 OID 208829)
-- Name: FK_task_task_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_ParentId" FOREIGN KEY (parent_id) REFERENCES task(id);


--
-- TOC entry 2457 (class 2606 OID 208834)
-- Name: FK_task_task_SequentialPredecessorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_SequentialPredecessorId" FOREIGN KEY (sequential_predecessor_id) REFERENCES task(id);


--
-- TOC entry 2462 (class 2606 OID 208839)
-- Name: FK_task_time_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2463 (class 2606 OID 208844)
-- Name: FK_task_time_user_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2464 (class 2606 OID 208849)
-- Name: FK_time_category_TimeCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_category_TimeCategoryId" FOREIGN KEY (time_category_id) REFERENCES time_category(id);


--
-- TOC entry 2465 (class 2606 OID 208854)
-- Name: FK_time_user_WorkerContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_WorkerContactId" FOREIGN KEY (worker_contact_id) REFERENCES contact(id);


--
-- TOC entry 2466 (class 2606 OID 208859)
-- Name: FK_version_asset_id_asset_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY version
    ADD CONSTRAINT "FK_version_asset_id_asset_id" FOREIGN KEY (asset_id) REFERENCES asset(id);


--
-- TOC entry 2478 (class 2606 OID 209242)
-- Name: activity_base_activity_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_base
    ADD CONSTRAINT activity_base_activity_type_id FOREIGN KEY (type) REFERENCES activity_type(id);


--
-- TOC entry 2479 (class 2606 OID 209247)
-- Name: activity_base_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_base
    ADD CONSTRAINT activity_base_contact_id FOREIGN KEY (owner) REFERENCES contact(id);


--
-- TOC entry 2480 (class 2606 OID 209252)
-- Name: activity_base_priority_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_base
    ADD CONSTRAINT activity_base_priority_id FOREIGN KEY (priority) REFERENCES activity_priority(id);


--
-- TOC entry 2481 (class 2606 OID 209257)
-- Name: activity_base_status_reason_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_base
    ADD CONSTRAINT activity_base_status_reason_id FOREIGN KEY (status_reason) REFERENCES activity_status_reason(id);


--
-- TOC entry 2482 (class 2606 OID 209271)
-- Name: activity_correspondence_base_activity_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_activity_type_id FOREIGN KEY (type) REFERENCES activity_type(id);


--
-- TOC entry 2483 (class 2606 OID 209276)
-- Name: activity_correspondence_base_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_contact_id FOREIGN KEY (owner) REFERENCES contact(id);


--
-- TOC entry 2484 (class 2606 OID 209281)
-- Name: activity_correspondence_base_direction; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_direction FOREIGN KEY (direction) REFERENCES activity_direction(id);


--
-- TOC entry 2485 (class 2606 OID 209286)
-- Name: activity_correspondence_base_priority_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_priority_id FOREIGN KEY (priority) REFERENCES activity_priority(id);


--
-- TOC entry 2486 (class 2606 OID 209291)
-- Name: activity_correspondence_base_recipient; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_recipient FOREIGN KEY (recipient) REFERENCES contact(id);


--
-- TOC entry 2487 (class 2606 OID 209296)
-- Name: activity_correspondence_base_sender; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_sender FOREIGN KEY (sender) REFERENCES contact(id);


--
-- TOC entry 2488 (class 2606 OID 209301)
-- Name: activity_correspondence_base_status_reason_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_correspondence_base
    ADD CONSTRAINT activity_correspondence_base_status_reason_id FOREIGN KEY (status_reason) REFERENCES activity_status_reason(id);


--
-- TOC entry 2489 (class 2606 OID 209315)
-- Name: activity_email_activity_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_activity_type_id FOREIGN KEY (type) REFERENCES activity_type(id);


--
-- TOC entry 2490 (class 2606 OID 209320)
-- Name: activity_email_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_contact_id FOREIGN KEY (owner) REFERENCES contact(id);


--
-- TOC entry 2491 (class 2606 OID 209325)
-- Name: activity_email_direction; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_direction FOREIGN KEY (direction) REFERENCES activity_direction(id);


--
-- TOC entry 2492 (class 2606 OID 209330)
-- Name: activity_email_priority_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_priority_id FOREIGN KEY (priority) REFERENCES activity_priority(id);


--
-- TOC entry 2493 (class 2606 OID 209335)
-- Name: activity_email_recipient; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_recipient FOREIGN KEY (recipient) REFERENCES contact(id);


--
-- TOC entry 2494 (class 2606 OID 209340)
-- Name: activity_email_sender; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_sender FOREIGN KEY (sender) REFERENCES contact(id);


--
-- TOC entry 2495 (class 2606 OID 209345)
-- Name: activity_email_status_reason_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_email
    ADD CONSTRAINT activity_email_status_reason_id FOREIGN KEY (status_reason) REFERENCES activity_status_reason(id);


--
-- TOC entry 2496 (class 2606 OID 209359)
-- Name: activity_letter_activity_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_activity_type_id FOREIGN KEY (type) REFERENCES activity_type(id);


--
-- TOC entry 2497 (class 2606 OID 209364)
-- Name: activity_letter_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_contact_id FOREIGN KEY (owner) REFERENCES contact(id);


--
-- TOC entry 2498 (class 2606 OID 209369)
-- Name: activity_letter_direction; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_direction FOREIGN KEY (direction) REFERENCES activity_direction(id);


--
-- TOC entry 2499 (class 2606 OID 209374)
-- Name: activity_letter_priority_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_priority_id FOREIGN KEY (priority) REFERENCES activity_priority(id);


--
-- TOC entry 2500 (class 2606 OID 209379)
-- Name: activity_letter_recipient; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_recipient FOREIGN KEY (recipient) REFERENCES contact(id);


--
-- TOC entry 2501 (class 2606 OID 209384)
-- Name: activity_letter_sender; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_sender FOREIGN KEY (sender) REFERENCES contact(id);


--
-- TOC entry 2502 (class 2606 OID 209389)
-- Name: activity_letter_status_reason_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_letter
    ADD CONSTRAINT activity_letter_status_reason_id FOREIGN KEY (status_reason) REFERENCES activity_status_reason(id);


--
-- TOC entry 2503 (class 2606 OID 209403)
-- Name: activity_phonecall_activity_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_activity_type_id FOREIGN KEY (type) REFERENCES activity_type(id);


--
-- TOC entry 2504 (class 2606 OID 209408)
-- Name: activity_phonecall_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_contact_id FOREIGN KEY (owner) REFERENCES contact(id);


--
-- TOC entry 2505 (class 2606 OID 209413)
-- Name: activity_phonecall_direction; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_direction FOREIGN KEY (direction) REFERENCES activity_direction(id);


--
-- TOC entry 2506 (class 2606 OID 209418)
-- Name: activity_phonecall_priority_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_priority_id FOREIGN KEY (priority) REFERENCES activity_priority(id);


--
-- TOC entry 2507 (class 2606 OID 209423)
-- Name: activity_phonecall_recipient; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_recipient FOREIGN KEY (recipient) REFERENCES contact(id);


--
-- TOC entry 2508 (class 2606 OID 209428)
-- Name: activity_phonecall_sender; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_sender FOREIGN KEY (sender) REFERENCES contact(id);


--
-- TOC entry 2509 (class 2606 OID 209433)
-- Name: activity_phonecall_status_reason_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_phonecall
    ADD CONSTRAINT activity_phonecall_status_reason_id FOREIGN KEY (status_reason) REFERENCES activity_status_reason(id);


--
-- TOC entry 2514 (class 2606 OID 209475)
-- Name: activity_regarding_base_activity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_regarding_base
    ADD CONSTRAINT activity_regarding_base_activity FOREIGN KEY (activity) REFERENCES activity_base(id);


--
-- TOC entry 2515 (class 2606 OID 209480)
-- Name: activity_regarding_base_type_FK; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_regarding_base
    ADD CONSTRAINT "activity_regarding_base_type_FK" FOREIGN KEY (type) REFERENCES activity_regarding_type(id);


--
-- TOC entry 2516 (class 2606 OID 209491)
-- Name: activity_regarding_lead_lead; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_regarding_lead
    ADD CONSTRAINT activity_regarding_lead_lead FOREIGN KEY (lead) REFERENCES lead(id);


--
-- TOC entry 2517 (class 2606 OID 209502)
-- Name: activity_regarding_opportunity_lead; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_regarding_opportunity
    ADD CONSTRAINT activity_regarding_opportunity_lead FOREIGN KEY (opportunity) REFERENCES opportunity(id);


--
-- TOC entry 2510 (class 2606 OID 209447)
-- Name: activity_task_activity_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_task
    ADD CONSTRAINT activity_task_activity_type_id FOREIGN KEY (type) REFERENCES activity_type(id);


--
-- TOC entry 2511 (class 2606 OID 209452)
-- Name: activity_task_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_task
    ADD CONSTRAINT activity_task_contact_id FOREIGN KEY (owner) REFERENCES contact(id);


--
-- TOC entry 2512 (class 2606 OID 209457)
-- Name: activity_task_priority_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_task
    ADD CONSTRAINT activity_task_priority_id FOREIGN KEY (priority) REFERENCES activity_priority(id);


--
-- TOC entry 2513 (class 2606 OID 209462)
-- Name: activity_task_status_reason_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY activity_task
    ADD CONSTRAINT activity_task_status_reason_id FOREIGN KEY (status_reason) REFERENCES activity_status_reason(id);


--
-- TOC entry 2425 (class 2606 OID 208864)
-- Name: fee_matter_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT "fee_matter_FeeId" FOREIGN KEY (fee_id) REFERENCES fee(id);


--
-- TOC entry 2426 (class 2606 OID 208869)
-- Name: fee_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY fee_matter
    ADD CONSTRAINT "fee_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2430 (class 2606 OID 208874)
-- Name: invoice_contact_BillToContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice
    ADD CONSTRAINT "invoice_contact_BillToContactId" FOREIGN KEY (bill_to_contact_id) REFERENCES contact(id);


--
-- TOC entry 2431 (class 2606 OID 208879)
-- Name: invoice_expense_expense_ExpenseId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT "invoice_expense_expense_ExpenseId" FOREIGN KEY (expense_id) REFERENCES expense(id);


--
-- TOC entry 2432 (class 2606 OID 208884)
-- Name: invoice_expense_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_expense
    ADD CONSTRAINT "invoice_expense_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2433 (class 2606 OID 208889)
-- Name: invoice_fee_fee_FeeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT "invoice_fee_fee_FeeId" FOREIGN KEY (fee_id) REFERENCES fee(id);


--
-- TOC entry 2434 (class 2606 OID 208894)
-- Name: invoice_fee_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_fee
    ADD CONSTRAINT "invoice_fee_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2435 (class 2606 OID 208899)
-- Name: invoice_time_invoice_InvoiceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT "invoice_time_invoice_InvoiceId" FOREIGN KEY (invoice_id) REFERENCES invoice(id);


--
-- TOC entry 2436 (class 2606 OID 208904)
-- Name: invoice_time_time_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY invoice_time
    ADD CONSTRAINT "invoice_time_time_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2470 (class 2606 OID 209111)
-- Name: lead_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead
    ADD CONSTRAINT lead_contact_id FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2471 (class 2606 OID 209116)
-- Name: lead_lead_fee_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead
    ADD CONSTRAINT lead_lead_fee_id FOREIGN KEY (fee_id) REFERENCES lead_fee(id);


--
-- TOC entry 2472 (class 2606 OID 209121)
-- Name: lead_lead_source_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead
    ADD CONSTRAINT lead_lead_source_id FOREIGN KEY (source_id) REFERENCES lead_source(id);


--
-- TOC entry 2467 (class 2606 OID 209074)
-- Name: lead_source_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead_source
    ADD CONSTRAINT lead_source_contact_id FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2468 (class 2606 OID 209079)
-- Name: lead_source_type_id_unique; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead_source
    ADD CONSTRAINT lead_source_type_id_unique FOREIGN KEY (type_id) REFERENCES lead_source_type(id);


--
-- TOC entry 2473 (class 2606 OID 209126)
-- Name: lead_status_lead_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead
    ADD CONSTRAINT lead_status_lead_status_id FOREIGN KEY (status_id) REFERENCES lead_status(id);


--
-- TOC entry 2469 (class 2606 OID 209095)
-- Name: load_fee_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY lead_fee
    ADD CONSTRAINT load_fee_contact_id FOREIGN KEY (to_id) REFERENCES contact(id);


--
-- TOC entry 2452 (class 2606 OID 208909)
-- Name: note_notification_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT "note_notification_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2453 (class 2606 OID 208914)
-- Name: note_notification_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_notification
    ADD CONSTRAINT "note_notification_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2474 (class 2606 OID 209152)
-- Name: opportunity_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity
    ADD CONSTRAINT opportunity_contact_id FOREIGN KEY (account_id) REFERENCES contact(id);


--
-- TOC entry 2475 (class 2606 OID 209157)
-- Name: opportunity_lead_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity
    ADD CONSTRAINT opportunity_lead_id FOREIGN KEY (lead_id) REFERENCES lead(id);


--
-- TOC entry 2476 (class 2606 OID 209162)
-- Name: opportunity_matter_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity
    ADD CONSTRAINT opportunity_matter_id FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2477 (class 2606 OID 209167)
-- Name: opportunity_opportunity_stage_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY opportunity
    ADD CONSTRAINT opportunity_opportunity_stage_id FOREIGN KEY (stage_id) REFERENCES opportunity_stage(id);


--
-- TOC entry 2408 (class 2606 OID 208919)
-- Name: profiledata_profile_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "ProfileData"
    ADD CONSTRAINT profiledata_profile_fkey FOREIGN KEY ("Profile") REFERENCES "Profiles"("pId") ON DELETE CASCADE;


--
-- TOC entry 2409 (class 2606 OID 208924)
-- Name: profiles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "Profiles"
    ADD CONSTRAINT profiles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2410 (class 2606 OID 208929)
-- Name: usersinroles_rolename_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_rolename_fkey FOREIGN KEY ("Rolename", "ApplicationName") REFERENCES "Roles"("Rolename", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2411 (class 2606 OID 208934)
-- Name: usersinroles_username_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "UsersInRoles"
    ADD CONSTRAINT usersinroles_username_fkey FOREIGN KEY ("Username", "ApplicationName") REFERENCES "Users"("Username", "ApplicationName") ON DELETE CASCADE;


--
-- TOC entry 2633 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2017-12-02 20:16:10

--
-- PostgreSQL database dump complete
--

