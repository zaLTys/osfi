create table osfi.Users (
 ID  number primary key,
 username varchar(50) not null,
 password varchar(256) not null,
 role varchar(20) not null,
 vardas varchar(50) not null,
 pavarde varchar(50)not null
);

create sequence osfi.Seq_User;

create table osfi.upload (
 ID  number primary key,
 IMONE_ID references osfi.Imone not null,
 Metai number not null,
 FileId varchar2(32) not null,
 data timestamp not null
);

create sequence osfi.Seq_upload;

create table osfi.UPLOADSTATUS (
 ID number primary key,
 UPLOAD_ID references osfi.upload not null,
 USER_ID references osfi.users null,
 BUKLE varchar (20) not null,
 data_nuo timestamp not null,
 data_iki timestamp null
);

create sequence osfi.Seq_UPLOADSTATUS;

--uzpildom uploadu lentele
insert into osfi.upload
select
 osfi.Seq_upload.nextval, ikelimai.imone_id, 2011, '00000000000000000000000000000000', ikelimai.datanuo
from
 (select
 distinct a.imone_id, a.datanuo
from
 osfi.augalininkyste a) ikelimai;

--suskaiciuojam uploadu bukles
insert into osfi.UPLOADSTATUS
select
 osfi.Seq_UPLOADSTATUS.nextval id,
 u.upload_id,
 NULL,
 'Patvirtintas',
 u.datanuo,
 u.dataiki
from
 (select
 distinct a.imone_id, a.datanuo, a.dataiki, u.id upload_id
from
   osfi.augalininkyste a
 , osfi.upload u
where 
     a.datanuo = u.data
 and a.imone_id = u.imone_id
order by 1, 2,3) u;
 
insert into osfi.UPLOADSTATUS
select
 osfi.Seq_UPLOADSTATUS.nextval id,
 u.upload_id,
 NULL,
 'Atmestas',
 u.data_iki,
 NULL
from
 osfi.UPLOADSTATUS u
where u.data_iki is not null;

--pridedam upload_id stulpelius prie visu formu
alter table osfi.augalininkyste add upload_id number null;
alter table osfi.darbuotojai add upload_id number null;
alter table osfi.dotacijossubsidijos add upload_id number null;
alter table osfi.formospildymolaikas add upload_id number null;
alter table osfi.gyvulininkyste add upload_id number null;
alter table osfi.gyvuliuskaicius add upload_id number null;
alter table osfi.ilgalaikisturtas add upload_id number null;
alter table osfi.produkcijoskaita add upload_id number null;
alter table osfi.produktupardavimas add upload_id number null;
alter table osfi.sanaudos add upload_id number null;
alter table osfi.zemesplotai add upload_id number null;
alter table osfi.imonesduomenys add upload_id number null;

update osfi.augalininkyste      t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.darbuotojai         t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.dotacijossubsidijos t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.formospildymolaikas t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.gyvulininkyste      t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.gyvuliuskaicius     t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.ilgalaikisturtas    t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 4);
update osfi.produkcijoskaita    t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.produktupardavimas  t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 4);
update osfi.sanaudos            t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.zemesplotai         t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 5);
update osfi.imonesduomenys      t set t.upload_id = (select u.id from osfi.upload u where t.imone_id = u.imone_id and abs(trunc((cast(t.datanuo as date) - cast(u.data as date)) * 86400)) < 4);

update osfi.formospildymolaikas set upload_ID = 332 where id = 9;
update osfi.formospildymolaikas set upload_ID = 33 where id = 437;
update osfi.zemesplotai set upload_ID = 332 where id in (95,96,97,98,99,100,101,102,103,104);
update osfi.zemesplotai set upload_ID = 33 where id in (3041,3042,3043,3044,3045,3046,3047,3048,3049,3050);
update osfi.dotacijossubsidijos set upload_ID = 33 where id in(2605,2606,2607,2608,2609,2610);
update osfi.gyvulininkyste set upload_ID = 33 where id in(9513,9514,9515,9516,9517,9518,9519,9520,9521,9522,9523,9524,9525,9526,9527,9528,9529,9530,9531,9532,9533,9534,9535);
update osfi.gyvuliuskaicius set upload_ID = 332 where id in(151,152,153,154,155,156,157,158,159,160,161,162,163,164,165);
update osfi.gyvuliuskaicius set upload_ID = 33 where id in(4701,4702,4703,4704,4705,4706,4707,4708,4709,4710,4711,4712,4713,4714,4715);
update osfi.produkcijoskaita set upload_ID = 33 where id in(9312,9313,9314,9315,9316,9317,9318,9319,9320,9321,9322,9323,9324,9325,9326,9327,9328,9329,9330,9331,9332,9333,9334,9335,9336,9337,9338,9339);
update osfi.produkcijoskaita set upload_ID = 332 where id in(288,289,290,291,292,293,294,295,296,297,298,299,300,301,302,303,304,305,306,307,308,309,310,311,312,313,314,315);

ALTER TABLE osfi.augalininkyste      add Constraint fk_augalininkyste_Upload_id 		Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.darbuotojai         add Constraint fk_darbuotojai_Upload_id	 		Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.dotacijossubsidijos add Constraint fk_dotsubs_Upload_id 	Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.formospildymolaikas add Constraint fk_formlaikas_Upload_id 	Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.gyvulininkyste      add Constraint fk_gyvulininkyste_Upload_id 		Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.gyvuliuskaicius     add Constraint fk_gyvuliuskaicius_Upload_id 		Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.ilgalaikisturtas    add Constraint fk_ilgalaikisturtas_Upload_id 		Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.produkcijoskaita    add Constraint fk_produkcijoskaita_Upload_id 		Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.produktupardavimas  add Constraint fk_prodpard_Upload_id 	Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.sanaudos            add Constraint fk_sanaudos_Upload_id 				Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.zemesplotai         add Constraint fk_zemesplotai_Upload_id 			Foreign Key (upload_ID) References osfi.upload(id);
ALTER TABLE osfi.imonesduomenys         add Constraint fk_imonesduomenys_Upload_id 			Foreign Key (upload_ID) References osfi.upload(id);

ALTER TABLE osfi.augalininkyste      drop column datanuo;
ALTER TABLE osfi.darbuotojai         drop column datanuo;
ALTER TABLE osfi.dotacijossubsidijos drop column datanuo;
ALTER TABLE osfi.formospildymolaikas drop column datanuo;
ALTER TABLE osfi.gyvulininkyste      drop column datanuo;
ALTER TABLE osfi.gyvuliuskaicius     drop column datanuo;
ALTER TABLE osfi.ilgalaikisturtas    drop column datanuo;
ALTER TABLE osfi.produkcijoskaita    drop column datanuo;
ALTER TABLE osfi.produktupardavimas  drop column datanuo;
ALTER TABLE osfi.sanaudos            drop column datanuo;
ALTER TABLE osfi.zemesplotai         drop column datanuo;
ALTER TABLE osfi.imonesduomenys      drop column datanuo;

ALTER TABLE osfi.augalininkyste      drop column dataiki;
ALTER TABLE osfi.darbuotojai         drop column dataiki;
ALTER TABLE osfi.dotacijossubsidijos drop column dataiki;
ALTER TABLE osfi.formospildymolaikas drop column dataiki;
ALTER TABLE osfi.gyvulininkyste      drop column dataiki;
ALTER TABLE osfi.gyvuliuskaicius     drop column dataiki;
ALTER TABLE osfi.ilgalaikisturtas    drop column dataiki;
ALTER TABLE osfi.produkcijoskaita    drop column dataiki;
ALTER TABLE osfi.produktupardavimas  drop column dataiki;
ALTER TABLE osfi.sanaudos            drop column dataiki;
ALTER TABLE osfi.zemesplotai         drop column dataiki;
ALTER TABLE osfi.imonesduomenys      drop column dataiki;

ALTER table osfi.upload add bukle varchar (20) null;
update osfi.upload u set u.bukle =
(
with orderingas as
 (
       select 1 nr, 'Patvirtintas' bukle from dual
 union select 2 nr, 'Nepatvirtintas' bukle from dual
 union select 3 nr, 'Atmestas' bukle from dual
 union select 4 nr, 'Netinkamas' bukle from dual
)
, suorderinti as
(
select
 us.upload_id, o.nr, us.bukle, rank() OVER(PARTITION BY us.upload_id ORDER BY o.nr) numeris
from
   osfi.uploadstatus us
 , orderingas o   
where
     us.data_iki is null
 and o.bukle = us.bukle
)
select
 s.bukle
from
 suorderinti s
where
     s.upload_id = u.id
 and s.numeris = 1
);

ALTER TABLE osfi.upload      modify bukle varchar (20) not null;

    create table osfi.Klaidos (
       Id number primary key,
       UPLOAD_ID references osfi.upload not null,
       FormosTipas NVARCHAR2(30) not null,
       IrasoKodas NVARCHAR2(5) not null,
       Stulpelis NUMBER(10,0) not null,
       KlaidosKodas NVARCHAR2(30)
    );
    
    create sequence osfi.Seq_KlaidosAprasas;
 
 insert into osfi.USERS  values (osfi.Seq_User.nextval,'povilas','1abdf74ec2a38deb5f7d550a1087325d96815adcea59a3fec4c27ad49f8f1a7b','Administratorius','Povilas','Ðimanskas'); 
commit;