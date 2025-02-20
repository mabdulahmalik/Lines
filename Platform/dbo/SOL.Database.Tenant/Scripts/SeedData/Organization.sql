insert [org].[FacilityType] ([Id],[CreatedAt],[ModifiedAt],[Order],[Active],[Name])
select '{b029687f-518a-455b-8fba-7dde88cccd3d}','2024-10-24 14:18:38.4272196',NULL,3,1,N'Assisted Living' UNION ALL
select '{94cf5b2d-07d1-4cdb-8141-de1801ac459e}','2024-10-24 14:21:15.4477303',NULL,2,1,N'Long-term Acute Care' UNION ALL
select '{572f6828-2359-48f2-aaf1-ec11e39b5912}','2024-10-24 14:18:02.9036386',NULL,1,1,N'Hospital'

insert [org].[FacilityRoomProperty] ([Id],[FacilityTypeId],[Order],[Name])
select '{32f733e3-9d14-4d79-8848-1772b6ca68ce}','{572f6828-2359-48f2-aaf1-ec11e39b5912}',1,N'Floor' UNION ALL
select '{007a98b1-2a7a-4233-bd70-63239f452993}','{94cf5b2d-07d1-4cdb-8141-de1801ac459e}',3,N'Unit' UNION ALL
select '{3ccc5d01-722d-44b4-b223-eab8c6998a08}','{572f6828-2359-48f2-aaf1-ec11e39b5912}',2,N'Wing'

insert [org].[FacilityRoomPropertyOption] ([Id],[PropertyId],[Order],[Text])
select '{0f17a98e-0de8-43d0-a319-3e9754105022}','{32f733e3-9d14-4d79-8848-1772b6ca68ce}',3,N'3rd' UNION ALL
select '{ebe294d8-ee4e-470c-81be-436174fdb13e}','{3ccc5d01-722d-44b4-b223-eab8c6998a08}',3,N'East' UNION ALL
select '{5cc10f91-fede-4a37-bf79-627001f62142}','{32f733e3-9d14-4d79-8848-1772b6ca68ce}',2,N'2nd' UNION ALL
select '{6150b0ce-5813-47f1-99e1-62d5fe4fcdbd}','{3ccc5d01-722d-44b4-b223-eab8c6998a08}',2,N'South' UNION ALL
select '{b5eb80a0-e854-4668-94fa-685306908852}','{007a98b1-2a7a-4233-bd70-63239f452993}',1,N'ER' UNION ALL
select '{57cff012-a5a9-490c-a601-782f1c84af07}','{3ccc5d01-722d-44b4-b223-eab8c6998a08}',4,N'West' UNION ALL
select '{ca5df649-8e3e-4348-9fee-7b04e9a42014}','{32f733e3-9d14-4d79-8848-1772b6ca68ce}',1,N'1st' UNION ALL
select '{84d231a3-c854-4035-b96c-978a8b8c1be5}','{3ccc5d01-722d-44b4-b223-eab8c6998a08}',1,N'North' UNION ALL
select '{529f34f9-d1e0-434d-a31c-abeeaff038cb}','{007a98b1-2a7a-4233-bd70-63239f452993}',4,N'Surgery' UNION ALL
select '{b7c2d89c-2ae1-418b-a0e9-af3d53779147}','{007a98b1-2a7a-4233-bd70-63239f452993}',2,N'ICU' UNION ALL
select '{82bed9c4-0868-4826-9151-cbff5f819232}','{007a98b1-2a7a-4233-bd70-63239f452993}',3,N'Therapy'

insert [org].[Routine] ([Id],[CreatedAt],[ModifiedAt],[Active],[Name],[Description],[PurposeId],[Settings])
select '{09ad5010-0c06-4fd0-8ca1-4c454a74f847}','2024-10-24 16:38:30.2935208',NULL,1,N'Dressing Change',N'Regular check-ups/clean-ups of central line dressings.','{5da586e7-1f69-414e-b477-c37fc3711357}',1 UNION ALL
select '{2cdf4d65-5494-41c0-9bbe-b78598d728c0}','2024-10-24 16:34:25.8481558',NULL,1,N'Line Check',N'Regular central line maintenance.','{2fc296b2-c6fe-4020-a811-be552304559a}',0

insert [org].[RoutineOrigin] ([RoutineId],[ProcedureId],[PropertyId],[PropertyValue])
select '{2cdf4d65-5494-41c0-9bbe-b78598d728c0}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',NULL UNION ALL
select '{09ad5010-0c06-4fd0-8ca1-4c454a74f847}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',NULL

insert [org].[RoutineTerminus] ([RoutineId],[ProcedureId],[PropertyId],[PropertyValue])
select '{2cdf4d65-5494-41c0-9bbe-b78598d728c0}','{f88b8373-36bb-4440-93f8-c389d6d500f1}',NULL,NULL UNION ALL
select '{09ad5010-0c06-4fd0-8ca1-4c454a74f847}','{f88b8373-36bb-4440-93f8-c389d6d500f1}',NULL,NULL

insert [org].[RoutineRecurrence] ([RoutineId],[Time],[Days])
select '{2cdf4d65-5494-41c0-9bbe-b78598d728c0}','00:30:00',N'[7,1,2,3,4,5,6]'

insert [org].[RoutineRolling] ([RoutineId],[IntervalUnit],[IntervalValue],[DelayUnit],[DelayValue])
select '{09ad5010-0c06-4fd0-8ca1-4c454a74f847}',3,7,3,7

insert [org].[Facility] ([Id],[TypeId],[CreatedAt],[ModifiedAt],[Order],[Archived],[Name],[TimeZone],[RequiredData],[Street],[City],[State],[ZipCode])
select '{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','{b029687f-518a-455b-8fba-7dde88cccd3d}','2024-10-24 17:08:55.4339825','2024-10-24 17:11:31.2206011',2,0,N'Sunrise Senior Living',N'US/Central',0,N'123 Sunrise Ave',N'Birmingham',N'AL',N'12345' UNION ALL
select '{ad84a0d5-f178-40a7-a525-23a7c167fc12}','{572f6828-2359-48f2-aaf1-ec11e39b5912}','2024-10-24 16:52:36.7363460','2024-10-24 16:55:01.7507970',1,0,N'Emory - Johns Creek',N'US/Eastern',1,N'123 Emory Way',N'Johns Creek',N'GA',N'12345'

insert [org].[FacilityProcedure] ([FacilityId],[ProcedureId],[Timestamp])
select '{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','{2dafa8b2-86c8-4b69-bd56-65627f829955}','2024-10-24 17:11:31.2105872' UNION ALL
select '{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','{0e1e8195-46f5-4548-a14b-b28ee4cff5fe}','2024-10-24 17:11:31.2105878'

insert [org].[FacilityPurpose] ([FacilityId],[PurposeId],[Timestamp])
select '{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','{64a9c0ca-fd59-4f37-8b6b-7e05b3a64ff0}','2024-10-24 17:11:31.2102050' UNION ALL
select '{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','{2fc296b2-c6fe-4020-a811-be552304559a}','2024-10-24 17:11:31.2102982' UNION ALL
select '{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','{04f7d7e9-259c-4b10-b8b7-cb2691ad50eb}','2024-10-24 17:11:31.2102978'

insert [org].[FacilityRoutine] ([Id],[FacilityId],[RoutineId],[Name])
select '{8327039e-ff3b-4a0a-a2a7-a6ce778bc549}','{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','{09ad5010-0c06-4fd0-8ca1-4c454a74f847}',N'Check on Dressings' UNION ALL
select '{c40964c2-ccb4-44e4-88ab-c8b69682a1c7}','{ad84a0d5-f178-40a7-a525-23a7c167fc12}','{09ad5010-0c06-4fd0-8ca1-4c454a74f847}',N'Round on Dressings' UNION ALL
select '{c133c7bf-969d-4122-be11-c9bf4d382469}','{ad84a0d5-f178-40a7-a525-23a7c167fc12}','{2cdf4d65-5494-41c0-9bbe-b78598d728c0}',N'Round on Lines'

insert [org].[FacilityRoom] ([Id],[FacilityId],[CreatedAt],[ModifiedAt],[Name])
select '{3cc8af02-0f2e-4af6-9dd3-518b165f425a}','{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','2024-10-24 17:09:48.8769122',NULL,N'1137' UNION ALL
select '{8f744075-6990-4078-a0fe-558efdcdc773}','{ad84a0d5-f178-40a7-a525-23a7c167fc12}','2024-10-24 16:57:34.4777554',NULL,N'C15' UNION ALL
select '{b71e1b2e-aadb-4cff-be69-65aaaa8ed0f6}','{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','2024-10-24 17:10:06.9737165',NULL,N'1212' UNION ALL
select '{afa1a290-ecc3-4a24-bf2e-d92fd9ae1f80}','{ad84a0d5-f178-40a7-a525-23a7c167fc12}','2024-10-24 16:56:21.4688981',NULL,N'A10' UNION ALL
select '{aefb389f-9bb2-4c11-b525-dc4ec2460c99}','{ad84a0d5-f178-40a7-a525-23a7c167fc12}','2024-10-24 16:56:43.7191758',NULL,N'B12' UNION ALL
select '{c7f3c551-64f1-46e0-b74d-f2ed16100948}','{5e0b2a4a-f786-417e-9f82-0d84e4b036f9}','2024-10-24 17:09:29.7050693',NULL,N'1025'

insert [org].[FacilityRoomPropertyValue] ([RoomId],[PropertyId],[OptionId])
select '{8f744075-6990-4078-a0fe-558efdcdc773}','{32f733e3-9d14-4d79-8848-1772b6ca68ce}','{0f17a98e-0de8-43d0-a319-3e9754105022}' UNION ALL
select '{8f744075-6990-4078-a0fe-558efdcdc773}','{3ccc5d01-722d-44b4-b223-eab8c6998a08}','{84d231a3-c854-4035-b96c-978a8b8c1be5}' UNION ALL
select '{afa1a290-ecc3-4a24-bf2e-d92fd9ae1f80}','{32f733e3-9d14-4d79-8848-1772b6ca68ce}','{ca5df649-8e3e-4348-9fee-7b04e9a42014}' UNION ALL
select '{afa1a290-ecc3-4a24-bf2e-d92fd9ae1f80}','{3ccc5d01-722d-44b4-b223-eab8c6998a08}','{6150b0ce-5813-47f1-99e1-62d5fe4fcdbd}' UNION ALL
select '{aefb389f-9bb2-4c11-b525-dc4ec2460c99}','{32f733e3-9d14-4d79-8848-1772b6ca68ce}','{5cc10f91-fede-4a37-bf79-627001f62142}' UNION ALL
select '{aefb389f-9bb2-4c11-b525-dc4ec2460c99}','{3ccc5d01-722d-44b4-b223-eab8c6998a08}','{57cff012-a5a9-490c-a601-782f1c84af07}'