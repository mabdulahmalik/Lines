insert [enc].[Purpose] ([Id],[CreatedAt],[ModifiedAt],[Order],[Name],[Archived])
select '{a2ccdc3d-59d2-4b63-84b9-060a2d8a4eca}','2024-10-24 14:39:07.0774695',NULL,7,N'Port Access',0 UNION ALL
select '{1619a519-f1ba-4c4f-b21f-4dc32eca00cd}','2024-10-24 14:39:00.2968912',NULL,1,N'IV',0 UNION ALL
select '{fc8073e9-ff6a-4998-9c7d-50a44a52c27c}','2024-10-24 14:38:15.4275989',NULL,2,N'PICC Line',0 UNION ALL
select '{64a9c0ca-fd59-4f37-8b6b-7e05b3a64ff0}','2024-10-24 14:34:49.4262472',NULL,4,N'Blood Culture',0 UNION ALL
select '{2fc296b2-c6fe-4020-a811-be552304559a}','2024-10-24 14:31:03.6373592',NULL,6,N'Line Check',0 UNION ALL
select '{511d58c7-8599-4158-810e-c072aa53d4b8}','2024-10-24 14:38:09.5194536',NULL,3,N'Midline',0 UNION ALL
select '{5da586e7-1f69-414e-b477-c37fc3711357}','2024-10-24 14:31:25.7772714',NULL,7,N'Dressing Change',0 UNION ALL
select '{04f7d7e9-259c-4b10-b8b7-cb2691ad50eb}','2024-10-24 14:33:21.5058212',NULL,5,N'Lab Draw',0

insert [enc].[Procedure] ([Id],[CreatedAt],[ModifiedAt],[Order],[Name],[Settings],[RequiredData],[Archived])
select '{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}','2024-10-24 15:55:59.4406975',NULL,6,N'Insertion Procedure',129,31,0 UNION ALL
select '{2dafa8b2-86c8-4b69-bd56-65627f829955}','2024-10-24 15:42:38.1509957',NULL,4,N'Line Check',128,0,0 UNION ALL
select '{0e1e8195-46f5-4548-a14b-b28ee4cff5fe}','2024-10-24 15:32:09.4726150',NULL,3,N'Lab Draw',128,0,0 UNION ALL
select '{f88b8373-36bb-4440-93f8-c389d6d500f1}','2024-10-24 16:28:49.6085654',NULL,7,N'PICC/CVL/Midline Removal',130,15,0 UNION ALL
select '{32a24e89-0a68-4c44-b999-c3acd0e60259}','2024-10-24 14:57:38.8266267',NULL,1,N'IV',128,0,0 UNION ALL
select '{d4d98eec-9bb4-4820-a914-e1e7ba9b8d32}','2024-10-24 15:24:09.9528951',NULL,2,N'Blood Culture',128,0,0 UNION ALL
select '{b2af0adb-42f9-4054-b834-e4e13c2a9411}','2024-10-24 15:47:36.0412768',NULL,5,N'Dressing Change',128,0,0

insert [enc].[ProcedureField] ([Id],[ProcedureId],[Order],[Type],[Settings],[Name],[Instruction],[Archived])
select '{747355e6-8d0d-407e-915e-168997671e9d}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',9,2,0,N'Insertion Length',N'',0 UNION ALL
select '{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',7,4,0,N'Vessel',N'',0 UNION ALL
select '{0a9e8a08-188b-4a46-b9c8-891a2b65f1ff}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',2,3,0,N'OSF insertion length unknown',N'',0 UNION ALL
select '{92b80a58-9581-415c-8dc8-a1e7fc5a3d09}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',4,1,0,N'Reason for Central Line',N'',0 UNION ALL
select '{c3ea276e-b729-42ef-a3a5-aaac5b87742c}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',1,1,0,N'Lot #',N'',0 UNION ALL
select '{56d77bd0-2c8c-4f40-bec8-ca9d45f5d09b}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',3,3,0,N'Unsuccessful',N'',0 UNION ALL
select '{faf5d79c-4d3b-488a-bf2c-d5a6fab6817c}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',5,3,0,N'Patient Refused',N'',0 UNION ALL
select '{609a157a-0fe8-4df7-bace-e53f163d504b}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',10,2,0,N'External cm',N'',0 UNION ALL
select '{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',6,4,1,N'Insertion Type',N'',0 UNION ALL
select '{8e9a0176-a0b4-4960-bf6e-fdff6c922b53}','{a8533b8c-0e9b-4da2-ab6b-2d0868188cc5}',8,4,0,N'Laterality',N'',0 UNION ALL
select '{899fdf93-ec27-405a-83fd-56806d733a27}','{2dafa8b2-86c8-4b69-bd56-65627f829955}',1,4,2,N'What',N'',0 UNION ALL
select '{9bbc1a25-b52c-4cb1-b5af-56b3013fa7d7}','{2dafa8b2-86c8-4b69-bd56-65627f829955}',2,2,0,N'External cm',N'',0 UNION ALL
select '{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}','{2dafa8b2-86c8-4b69-bd56-65627f829955}',3,4,2,N'Intervention',N'',0 UNION ALL
select '{03bec218-5866-4ac7-a50e-41ab4ef4b456}','{0e1e8195-46f5-4548-a14b-b28ee4cff5fe}',2,4,0,N'Draw Type',N'',0 UNION ALL
select '{823ccb61-ca3f-414d-a40c-4ce96f0ff397}','{0e1e8195-46f5-4548-a14b-b28ee4cff5fe}',5,3,0,N'Unsuccessful',N'',0 UNION ALL
select '{c298aa38-d14d-440b-a82d-943ca06aff6e}','{0e1e8195-46f5-4548-a14b-b28ee4cff5fe}',3,3,0,N'Ultrasound',N'',0 UNION ALL
select '{95b86534-db1c-4cfa-be04-96c69d072516}','{0e1e8195-46f5-4548-a14b-b28ee4cff5fe}',4,3,0,N'Hospital Staff Obtained',N'',0 UNION ALL
select '{b8a29f7d-333e-4efb-97f3-a028778066e7}','{0e1e8195-46f5-4548-a14b-b28ee4cff5fe}',1,3,0,N'Patient Refused',N'',0 UNION ALL
select '{ec3e40c1-6b0e-4785-8b1c-b3133972ea15}','{f88b8373-36bb-4440-93f8-c389d6d500f1}',2,2,0,N'Removal Length',N'Only specify is removed by VAT',0 UNION ALL
select '{af7c71b9-33f4-4f50-857a-f6ac1b3a8219}','{f88b8373-36bb-4440-93f8-c389d6d500f1}',1,3,0,N'Removed by VAT?',N'',0 UNION ALL
select '{c63aa7f0-c402-4024-926a-54439ce614a3}','{f88b8373-36bb-4440-93f8-c389d6d500f1}',3,4,2,N'Removal',N'',0 UNION ALL
select '{44577dca-a09d-4291-9f6b-012097720d6a}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',9,4,2,N'DC IV',N'',0 UNION ALL
select '{13edf772-fbd5-43d1-baad-8eb07fcb5ecd}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',2,4,0,N'Laterality',N'',0 UNION ALL
select '{bf584850-6cc0-4ddb-b0e1-8f9413f6bc61}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',5,3,0,N'Hospital Staff Obtained',N'',0 UNION ALL
select '{bdf9e76f-8577-4483-aa11-521dc3ff6311}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',3,4,0,N'Size',N'',0 UNION ALL
select '{33404d14-e013-4f17-aab3-2b16882e8db4}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',8,4,2,N'Dressing',N'',0 UNION ALL
select '{25faaad8-3c9a-430f-9094-6f9213adde42}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',7,3,0,N'Patient Refused',N'',0 UNION ALL
select '{f9ac5101-5c09-4326-af4d-a5d4622773ee}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',6,3,0,N'Unsuccessful',N'',0 UNION ALL
select '{79d5808e-8999-431c-badb-be830c75a976}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',4,3,0,N'Ultrasound',N'',0 UNION ALL
select '{e131d3c3-a7e0-40da-ab88-ff99af739dc2}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',1,4,0,N'Site',N'',0 UNION ALL
select '{bf14a68b-3a19-4d56-af59-c78a19173d68}','{32a24e89-0a68-4c44-b999-c3acd0e60259}',10,4,2,N'DC ML',N'',0 UNION ALL
select '{7017cb84-8a93-440c-a09f-c39b78b507a2}','{d4d98eec-9bb4-4820-a914-e1e7ba9b8d32}',3,4,2,N'From',N'',0 UNION ALL
select '{87045712-d7e7-478c-9462-a2683a27f62d}','{d4d98eec-9bb4-4820-a914-e1e7ba9b8d32}',6,3,0,N'Unsuccessful',N'',0 UNION ALL
select '{c01f58e1-1f12-40c7-b883-fb485c061b54}','{d4d98eec-9bb4-4820-a914-e1e7ba9b8d32}',2,4,0,N'Laterality',N'',0 UNION ALL
select '{e88bee4a-0de0-4169-aaf4-416fa2635359}','{d4d98eec-9bb4-4820-a914-e1e7ba9b8d32}',1,3,0,N'Ultrasound',N'',0 UNION ALL
select '{ce4b8e72-49ca-4402-a07b-4849b0a3f079}','{d4d98eec-9bb4-4820-a914-e1e7ba9b8d32}',4,3,0,N'Patient Refused',N'',0 UNION ALL
select '{9b387702-71eb-461d-a96b-5cc9c0030c73}','{d4d98eec-9bb4-4820-a914-e1e7ba9b8d32}',5,3,0,N'Hospital Staff Obtained',N'',0 UNION ALL
select '{424162b5-bb32-49e8-9276-5f0ac53c5e4d}','{b2af0adb-42f9-4054-b834-e4e13c2a9411}',2,4,0,N'Why',N'',0 UNION ALL
select '{8973083a-20fd-40f1-ac75-a3ec3ecd0e35}','{b2af0adb-42f9-4054-b834-e4e13c2a9411}',1,4,0,N'What',N'',0

insert [enc].[ProcedureFieldOption] ([Id],[FieldId],[Order],[Text],[Archived])
select '{70cd12cd-8e45-4a70-b0d5-10be42b89f6e}','{44577dca-a09d-4291-9f6b-012097720d6a}',2147483647,N'PT Removal',0 UNION ALL
select '{bf369652-45cd-492e-9b6a-166a45e88a80}','{44577dca-a09d-4291-9f6b-012097720d6a}',2147483647,N'Leaking',0 UNION ALL
select '{6a5c9ebd-e048-42b2-8381-5b1c5d2f40ce}','{44577dca-a09d-4291-9f6b-012097720d6a}',2147483647,N'Per Protocol',0 UNION ALL
select '{fb65d1cc-a405-4259-bd15-84ab559c2be1}','{44577dca-a09d-4291-9f6b-012097720d6a}',2147483647,N'Painful',0 UNION ALL
select '{749efaa1-32c9-4c1c-b273-d732fb143d8b}','{44577dca-a09d-4291-9f6b-012097720d6a}',2147483647,N'Phlebitis',0 UNION ALL
select '{6287e12a-34ad-460e-a106-e70c905fb2c0}','{44577dca-a09d-4291-9f6b-012097720d6a}',2147483647,N'Inflitration',0 UNION ALL
select '{be1b44d3-755b-43bb-b0ef-befaa531de78}','{33404d14-e013-4f17-aab3-2b16882e8db4}',2147483647,N'Dressing Reinforced',0 UNION ALL
select '{dc19f2b9-4117-44e7-96d0-02c9397387fe}','{33404d14-e013-4f17-aab3-2b16882e8db4}',2147483647,N'Dressing Changed',0 UNION ALL
select '{50345653-4085-487d-bcdf-178a1f54268c}','{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}',2147483647,N'Line Retracted',0 UNION ALL
select '{4839ffb6-990b-4467-9a1e-4bf2ebecba1b}','{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}',2147483647,N'Dressing Reinforced',0 UNION ALL
select '{91d41fe4-2964-48b3-965f-98cba8272024}','{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}',2147483647,N'Repositioned',0 UNION ALL
select '{de7a94fd-e349-44fa-aeb5-8922ae5b8f2d}','{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}',2147483647,N'Clave Change',0 UNION ALL
select '{eb3311f3-d05c-4de1-bc27-96abb63344d6}','{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}',2147483647,N'Power Flush',0 UNION ALL
select '{330a758f-874a-40c1-bd3d-e45154db4ae9}','{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}',2147483647,N'Recommend DC; note left in EPIC',0 UNION ALL
select '{4bf1feb5-7c8d-4884-83d7-d6a1dc8ff25f}','{0f29ecd7-46b4-4bcb-871d-3b0c784ec7f7}',2147483647,N'Heparin Lock',0 UNION ALL
select '{10e0a931-17d6-431e-9149-d884d0e426f6}','{03bec218-5866-4ac7-a50e-41ab4ef4b456}',2147483647,N'From IV',0 UNION ALL
select '{a19204ee-789f-40f0-b55f-f7b43646d27d}','{03bec218-5866-4ac7-a50e-41ab4ef4b456}',2147483647,N'From ML',0 UNION ALL
select '{e269194a-323f-42f3-8668-8b71831e8a41}','{03bec218-5866-4ac7-a50e-41ab4ef4b456}',2147483647,N'From PICC',0 UNION ALL
select '{d092e97c-da9c-4fe4-bd98-474b0deac5dc}','{03bec218-5866-4ac7-a50e-41ab4ef4b456}',2147483647,N'Port-A-Cath',0 UNION ALL
select '{c95f57fd-1c52-477d-8808-0c300aedaf39}','{03bec218-5866-4ac7-a50e-41ab4ef4b456}',2147483647,N'Labs Only',0 UNION ALL
select '{6af85c03-483f-4239-a431-0d3afb861ade}','{bdf9e76f-8577-4483-aa11-521dc3ff6311}',2147483647,N'24g',0 UNION ALL
select '{ee0370bd-0032-4a04-9870-7dec893f5c6d}','{bdf9e76f-8577-4483-aa11-521dc3ff6311}',2147483647,N'22g 1"',0 UNION ALL
select '{d04d978d-26a7-4ae5-a727-97ac0cb2c198}','{bdf9e76f-8577-4483-aa11-521dc3ff6311}',2147483647,N'22g 1.75"',0 UNION ALL
select '{e84e00b6-e3c4-4e25-8c30-dc29cfe35ffd}','{bdf9e76f-8577-4483-aa11-521dc3ff6311}',2147483647,N'20g',0 UNION ALL
select '{3fc00c38-52a6-424f-9c6b-de351b8632c0}','{bdf9e76f-8577-4483-aa11-521dc3ff6311}',2147483647,N'18g',0 UNION ALL
select '{4100bc56-1cce-4773-b7df-f213871b1a9f}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Painful/Pt Request',0 UNION ALL
select '{16ac7eea-7594-4b4f-a0e1-8d8706f67c9e}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Clotted',0 UNION ALL
select '{9c34a83a-dc29-4711-9b11-98eeade2fd28}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Discharge',0 UNION ALL
select '{3b889b0b-f9f3-4906-940c-a6e03af249bc}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Contaminated',0 UNION ALL
select '{0a042cec-40da-4cf7-b1e3-b350702d3af5}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Infiltration',0 UNION ALL
select '{cfbbae1d-4e25-49eb-b26a-022fdab9b19e}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Leaking',0 UNION ALL
select '{161016b4-7453-4027-a78a-18f8fe06bed4}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Hospital Staff Removed',0 UNION ALL
select '{2f0630ef-1d8f-4b60-a255-227b0e551df4}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Per MD Order',0 UNION ALL
select '{1ced6dc2-9664-471c-a83e-46e0025b33ed}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Phlebitis',0 UNION ALL
select '{d80ea27e-174c-400a-a0e6-4bcf06e3f722}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Kinked',0 UNION ALL
select '{5affe347-fdaf-49a5-ba72-58c177420d65}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Pt Removal',0 UNION ALL
select '{68b8feee-b7e3-4370-afcf-79f1dca8adbd}','{c63aa7f0-c402-4024-926a-54439ce614a3}',2147483647,N'Therapy Complete',0 UNION ALL
select '{904ca19c-fdbc-424a-9712-61f17bed31f4}','{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}',2147483647,N'Internal Jugular',0 UNION ALL
select '{632d8b5b-9131-405b-8bbd-bd98b521c881}','{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}',2147483647,N'Femoral',0 UNION ALL
select '{53d7bf98-c391-4caf-807b-9d9cd5c2db06}','{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}',2147483647,N'Cephalic',0 UNION ALL
select '{53722eee-5516-4ed1-83c2-8e6a52cc0d73}','{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}',2147483647,N'Subclavian',0 UNION ALL
select '{0bf6b740-9fd9-4315-a06d-8b4813353893}','{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}',2147483647,N'Basilic',0 UNION ALL
select '{fe6395bd-749f-4929-83d9-c8ba079e8222}','{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}',2147483647,N'Brachial',0 UNION ALL
select '{6d48e06c-6530-4e85-9ad8-c8bd4c8964b5}','{1fc8b628-1c6a-4ed7-a0a5-5445d949c876}',2147483647,N'Axillary',0 UNION ALL
select '{b318e70d-8ef2-4332-b130-e6f2a2dcb77e}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'CVL',0 UNION ALL
select '{5a561ab0-e88f-47f8-9a3e-f6dd979c6742}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'PORT',0 UNION ALL
select '{a359a587-b43a-4620-98da-92361d2dc4d3}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'IV',0 UNION ALL
select '{fa2d1a33-5d80-4f5b-94ac-8628f8ab37ce}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'ML',0 UNION ALL
select '{51813022-6b00-4702-81f2-8c210a4ccc8c}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'PICC',0 UNION ALL
select '{15bb139f-af17-491b-98a5-a01caa654264}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'PG',0 UNION ALL
select '{4024cdb9-8946-4a3a-9b12-5ffa72201f0d}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'Introducer',0 UNION ALL
select '{6cb18714-e0da-42af-8ef7-41aa6ae25169}','{899fdf93-ec27-405a-83fd-56806d733a27}',2147483647,N'JACC',0 UNION ALL
select '{456b2043-5736-40a0-8954-4b2dccbf8a5b}','{424162b5-bb32-49e8-9276-5f0ac53c5e4d}',2147483647,N'Site Assessment',0 UNION ALL
select '{caa53b7d-963a-4d16-bba8-60089a4ee839}','{424162b5-bb32-49e8-9276-5f0ac53c5e4d}',2147483647,N'Per Protocol',0 UNION ALL
select '{c3e3766f-f329-4be0-9268-00e642075a65}','{424162b5-bb32-49e8-9276-5f0ac53c5e4d}',2147483647,N'Bleeding',0 UNION ALL
select '{054c4afc-1f02-4823-9b17-b5839ea89ab4}','{424162b5-bb32-49e8-9276-5f0ac53c5e4d}',2147483647,N'Dressing changed by ICU nursing staff',0 UNION ALL
select '{a3b93e6a-3ebe-4e16-8beb-8c41ae44fa52}','{424162b5-bb32-49e8-9276-5f0ac53c5e4d}',2147483647,N'Comfort Care (not changed)',0 UNION ALL
select '{2e0aa0e6-2ded-4834-b40e-d8cc38b71678}','{424162b5-bb32-49e8-9276-5f0ac53c5e4d}',2147483647,N'Dressing Compromised',0 UNION ALL
select '{b17c1616-54a9-4d4f-a273-04d43e891b8e}','{13edf772-fbd5-43d1-baad-8eb07fcb5ecd}',2147483647,N'Left',0 UNION ALL
select '{acf90bd0-b457-4bad-ae46-14c4a8438dfe}','{13edf772-fbd5-43d1-baad-8eb07fcb5ecd}',2147483647,N'Right',0 UNION ALL
select '{dc3503b8-e24a-4e13-ba52-06e66b5275b0}','{8973083a-20fd-40f1-ac75-a3ec3ecd0e35}',2147483647,N'ML',0 UNION ALL
select '{d846ca73-9451-47d9-b2f4-84171975776c}','{8973083a-20fd-40f1-ac75-a3ec3ecd0e35}',2147483647,N'Introducer',0 UNION ALL
select '{bb90fc44-fec2-48a3-a3f7-ad6abb7a5421}','{8973083a-20fd-40f1-ac75-a3ec3ecd0e35}',2147483647,N'PG',0 UNION ALL
select '{c43272d4-968b-4688-a71e-cdb0218000b7}','{8973083a-20fd-40f1-ac75-a3ec3ecd0e35}',2147483647,N'PICC',0 UNION ALL
select '{865e81c4-603a-4d42-a89c-c81ca27755c8}','{8973083a-20fd-40f1-ac75-a3ec3ecd0e35}',2147483647,N'Central Line',0 UNION ALL
select '{90da1934-3a6c-4ff1-9a87-f63a73742e65}','{8973083a-20fd-40f1-ac75-a3ec3ecd0e35}',2147483647,N'Port-A-Cath',0 UNION ALL
select '{9c520aa3-d88f-4b16-9e15-b8927688e6a3}','{7017cb84-8a93-440c-a09f-c39b78b507a2}',2147483647,N'Peripheral Stick',0 UNION ALL
select '{2e2d208e-0766-49af-882a-9883a9e99536}','{7017cb84-8a93-440c-a09f-c39b78b507a2}',2147483647,N'PICC',0 UNION ALL
select '{cfcda1d2-ce6a-4f81-905e-8ebdd661e0e4}','{7017cb84-8a93-440c-a09f-c39b78b507a2}',2147483647,N'Port-a-Cath',0 UNION ALL
select '{0d8ca3e0-407a-4ba7-99ad-256499ba0b79}','{7017cb84-8a93-440c-a09f-c39b78b507a2}',2147483647,N'IV',0 UNION ALL
select '{8e103727-db1a-40ae-aa75-698923b1902c}','{7017cb84-8a93-440c-a09f-c39b78b507a2}',2147483647,N'Central Line',0 UNION ALL
select '{f5b4224c-3e6a-4355-a1a9-51726bc3b56a}','{7017cb84-8a93-440c-a09f-c39b78b507a2}',2147483647,N'ML',0 UNION ALL
select '{452cacbc-2ded-45cf-8954-3257aea4eed1}','{bf14a68b-3a19-4d56-af59-c78a19173d68}',2147483647,N'Phlebitis',0 UNION ALL
select '{57ead5df-7901-424e-ac47-18b8baaf7bdf}','{bf14a68b-3a19-4d56-af59-c78a19173d68}',2147483647,N'Per Protocol',0 UNION ALL
select '{79513e2f-e2af-453e-a6ce-8c5a3a6b414d}','{bf14a68b-3a19-4d56-af59-c78a19173d68}',2147483647,N'PT Removal',0 UNION ALL
select '{a7d76880-4a6d-47e1-87ec-9babf99d1096}','{bf14a68b-3a19-4d56-af59-c78a19173d68}',2147483647,N'Leaking',0 UNION ALL
select '{5225e564-fa41-4171-a581-f3b4d924f55f}','{bf14a68b-3a19-4d56-af59-c78a19173d68}',2147483647,N'Painful',0 UNION ALL
select '{c96278fa-eefe-4d1c-b3fb-da8a25707225}','{bf14a68b-3a19-4d56-af59-c78a19173d68}',2147483647,N'Inflitration',0 UNION ALL
select '{7711039e-c05b-46c9-bf47-fb567107a0a0}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'TL PICC',0 UNION ALL
select '{814f46aa-db9e-4c7e-945a-a87a2b655084}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'Introducer',0 UNION ALL
select '{fbe68fb4-85f3-4047-9505-abf7b9b67340}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'ML',0 UNION ALL
select '{bd7831b4-427d-414b-a476-bea33d1e1a88}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'SL PICC',0 UNION ALL
select '{5b60f77d-8c33-443e-b449-c16db22fbb9a}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'CVL',0 UNION ALL
select '{5d8db79a-3312-45dd-bfc9-95b8393d8f8f}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'DL PICC',0 UNION ALL
select '{7f581bc6-b6a3-4248-b2a8-1636fa183e14}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'JACC',0 UNION ALL
select '{b6317cf8-4a97-49ee-9b8d-085373eeb5c6}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'Hickman',0 UNION ALL
select '{605a53cc-5faf-4681-aa80-76543d981467}','{eb73e10f-84a8-49c0-bfc4-f3667baf51e6}',2147483647,N'PG',0 UNION ALL
select '{718201ba-3c6c-4a18-9b7b-c156ad7e4edf}','{c01f58e1-1f12-40c7-b883-fb485c061b54}',2147483647,N'Right',0 UNION ALL
select '{b95f3915-cfb3-48dc-8b41-be77bc403682}','{c01f58e1-1f12-40c7-b883-fb485c061b54}',2147483647,N'Left',0 UNION ALL
select '{5a3e535b-4c56-4659-a16d-36b2292ea3b8}','{8e9a0176-a0b4-4960-bf6e-fdff6c922b53}',2147483647,N'Right',0 UNION ALL
select '{47cb7cc1-16f7-439a-90e6-3af5c347b7d7}','{8e9a0176-a0b4-4960-bf6e-fdff6c922b53}',2147483647,N'Left',0 UNION ALL
select '{881bb7f1-c8e3-4b3c-86a4-6ef980bacb38}','{e131d3c3-a7e0-40da-ab88-ff99af739dc2}',2147483647,N'AC',0 UNION ALL
select '{20eb7f34-e297-4f27-983d-3057cdcb5e1f}','{e131d3c3-a7e0-40da-ab88-ff99af739dc2}',2147483647,N'FA',0 UNION ALL
select '{c8f599c7-2132-4939-8f7b-2374612f27a8}','{e131d3c3-a7e0-40da-ab88-ff99af739dc2}',2147483647,N'UA-basilic',0 UNION ALL
select '{1f3e6396-895b-4779-8cf0-b2280e25b4d5}','{e131d3c3-a7e0-40da-ab88-ff99af739dc2}',2147483647,N'Leg',0 UNION ALL
select '{f146d039-99b7-4df0-9eed-d1573bd8a418}','{e131d3c3-a7e0-40da-ab88-ff99af739dc2}',2147483647,N'Hand',0 UNION ALL
select '{3fb4c2a2-22e3-425e-a449-e1d407500e55}','{e131d3c3-a7e0-40da-ab88-ff99af739dc2}',2147483647,N'UA-cephalic',0 UNION ALL
select '{8302b585-7782-4f87-8350-ebb0dccfe7a3}','{e131d3c3-a7e0-40da-ab88-ff99af739dc2}',2147483647,N'UA-brachial',0