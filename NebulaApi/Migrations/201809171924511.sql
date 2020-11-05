﻿ALTER TABLE [dbo].[Categories] ADD [IsActive] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Categories] ADD [CreatedDate] [datetime] NOT NULL DEFAULT '1900-01-01T00:00:00.000'
ALTER TABLE [dbo].[CookingDishes] ADD [IsActive] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[CookingDishes] ADD [CreatedDate] [datetime] NOT NULL DEFAULT '1900-01-01T00:00:00.000'
ALTER TABLE [dbo].[Customs] ADD [IsActive] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Customs] ADD [CreatedDate] [datetime] NOT NULL DEFAULT '1900-01-01T00:00:00.000'
ALTER TABLE [dbo].[SubCategories] ADD [IsActive] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[SubCategories] ADD [CreatedDate] [datetime] NOT NULL DEFAULT '1900-01-01T00:00:00.000'
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201809171924511__', N'NebulaApi.Migrations.Configuration',  0x1F8B0800000000000400ED5D596FE4B8117E0F90FF20F4531278BB7DEC0C26037B179EF678D3D8F101B76792378396E8B6323A7A75786D04F96579C84FCA5F082951126F91925ADDDE1D0C3070F3F8582C16C92259AAFADF7FFE7BFCE37318384F3049FD383A991C4CF7270E8CDCD8F3A3D5C924CF1EBE7B37F9F1873FFEE1F8A3173E3B5FAA7247B81CAA19A52793C72C5BBF9FCD52F71186209D86BE9BC469FC904DDD389C012F9E1DEEEFFF7576703083086282B01CE7F8268F323F84C50FF4731E472E5C6739082E620F0629494739CB02D5B904214CD7C08527934B789F07E074ED4FCBB213E734F001A2630983878903A228CE4086A87CFF3985CB2C89A3D5728D124070FBB286A8DC03085248A87FDF1437EDC8FE21EEC8ACA95841B9799AC5A125E0C111E1CC8CAFDE89BF939A7388771F118FB317DCEB827F279339C8E02A4E5E260EDFD8FB7990E082227BA755A53DA7CEDAAB0501C90BFEB7E7CCF320CB137812C13C4B40B0E75CE7F781EFFE0C5F6EE3AF303A89F220A08943E4A13C2601255D27F11A26D9CB0D7C20242FBC893363EBCDF88A7535AA4ED99945941D1D4E9C4BD438B80F603DF654C797599CC09F600413D44FEF1A64194C228C010BEE09AD736DE1FFABD690B0A15933712EC0F32718ADB2C79309FA73E29CFBCFD0AB5208059F231F4D3254294B72D8D6C8E724588460B5F98616E9A99BF94F75431FE23880209270508F334F20E6E619FAAF82C27FDFFA98595AACE35923B57A598EE3AF8809677EFA6827CE4DBD6F122D6B0B736699512387D7D529956A2B0B7118A2967F07B28BB02EC193BF2A8681472D2474E2DCC0A0C84E1FFD35616D9975470926449BDA7912873771505765F3EF6E41B28298A9B1A6D032CE13D782C87232C9486C60EFCA4214795C5EDD6A4D1A5FA0A2BDDBBC278CB499F245D16FB35D3E6BAED6A8B2D777D6DCE2B29779780F9336E25FF5346627E950B3599832BA29DF69DA58EF93DF3648A564A1F1F6D3CD6F68A8F028BBE613F00B56F59D717F8F93AFE963BCC6288CEEC06658A2DEF9DEC699009FB3C5E65BC12C589C6DBC997B90E0C21B6F271AE3E4831B39C7EBC8A61BFA727ABB790180418090AF13DF6DF624E8FA2140FDBB4ED05FE412E6DDC459BA004F91C336CC3C853F2571BEBE409B831FDDE4FDA7B19F9E066EFC18077D814009331F4318E35F2391031B6B2D8C3DFFC16F942673C5825FDCEE9EFA694B7EBA84C913254F5D072BCF475867FDF40C0630EB4DEC28EB9B9F7E808D326B4CAA5A75AC2FDF245A63712E6A4A34EA229321E8896CAE4C41D451B4CCEFDB89620A7174517972D2E802BDD457860AB5168B3AF84FE86657890793F338F62A6596AAFE4DA795B535CA35E6F68F76C6F256F10DCBB24EE02EEACBF8D3747D09B36955715A429E2708EE57A4FF4E69C43DC7B85E23A387A6327A7470FF70F4EECD5BE01DBDFD1E1EBD195F5E25127470F8CE48822C45A1456E0FDFBC1DA455E512FA3945A3235D3CE9F1BE23C59AE553CC15165049915E4B688587A18617EB0A75F7451B532A8AB7B428EE509799503531F66CA8E8DD6CBBC61277BA5EA3C12B440B73C4EAFA89ABFB1A77EDF1C6FD63087CDD11D974193468651E470F7E12F6BF30BE06698A5601EF6F005F4C6E58F35842374F90702E3310AE37DEDAF5631C7177E163B435D8D0DCFE1A9F031729941F235CAB37DEA7D8FD1AE7D9C7A850D93E67AEA8B519020C42CEA9EBC2343D47C20CBD799C374F93DD8EE07879DAB61A320F801FCAF5106E21BDAB8A36BA88BC84A08F288AD91E3A3FC52B3F3223B52AAA26B52CD14A2A29664B2A0633A3949454135A1468A5B32C359896578CD0F06A5E01BBFB7ADEAE1FB9B7A52416C3C7BCD16C6A6F2A5AFA02827CE8A63ACD866211187E3614B0BB3F1B0A3251F293EF61ADC4E0F0531546F046E5E5E7AAF639C75136F67460BA3976E3E3AC01EAE992879C890031F15AA4E7015835B6B21637AE18346DCCC50638B8A1BE22F0E005F1865E9B59C65E40AC02573D295E19105F8BC50771511807A6F822C22FD47569F1CD8D297D0381F752173ED217BE05A81B75E1EFF585E7207261502A1B37F0971CA67427DE88835A0E9F6648D997F7DEA34AC36D65607FF633F7916268CBC07E00896A54E5EC3B4DD3D8F50B6630E6669C390FDB243AD73826B63D8D414465C37681B8E46315104DCE93C9FE742AF6A705BC3603A4C069C358B685BFF03CA0FAAB6703FB3EA52251F158C55A91B610A5019475B67E146AE3A56D67E917272D79D247B09E5D96BD9B5198CC6BD8801D17CC3C95B2A8B4F9EC22877A6849FF6598BD3A2EB9AC57D1A7BBB96F2864DF8F584A0F586C847E15953B9683DFC7B005C81CA42EF044F5012D5A9E056112D689AF006D4383343C98E04A005FFEA56839F7A34C5407FDC8F5D72068E51257D3F0E886FB5EB7C1E79CC1358C7083AD9C30695C7EE98F09A8DBE106A58D431682A8B8AD518D79DBD54D33EEC25DFC2832D97267A4904B726FB111C1D4736C04E1D4B3C48400E503D6360494DCD1990A007F61B76B02CADD142A04945C258C22A02CC7B620A02C4B5E9D809657B3A6E3CFDDD3EE9A78B217C4E36FEB5A766D4136197EEC986896772EA84E866AD44752AA0767F738133E67929338A2931CC65372C5C38B08065FC28C352EF4F12B4473D9231C9204AD9B43613F5A11816895BE0DABE89214859C7E5B0094541835DF1C91E43C618E502D58E47547C06095FC16107E86EA009B59DC024AEC8A042061B5B220AE7A20D4524754340BD8EA314F0B4B36560E969A60BC7CF1DF59514575DF63F1B3DFF00AA7EE1F25DEC24A6278634363B17DE0F707B6FB06ACE18C89459E686E730CEE7328CAE5241B5CE1D0BDA7A6E9305D67E6B6A2F7CAEB1DB30B9E0E3C90DDE95030DC82D59B13E227BB92B9A1BDEF31BBF1319563B34B9E56BE76E084CC1253E445DB1590E92510D503B2CC6BF8A0B9B3A170245B476FA6A8CC4244C6985C49D85C4A501D2383A16150CB05828249556706E752B589B57349762EB63919F7E212778A5570A9EACCE05C2232DACE24C9D9CCE274D68B45EC496AA0C9563DB4D64A7F9D773C2B1D199184E399C2E3D1F10558AFD1CA4879402229CEB2747F34FF6E69EF19282C31662EC36DFE8852B794C50958412E17AFDA1E3CF793343B0319B807F80971EE854231E91147A128564D0AA71871242BB5B1AA82FF261F01A85C16498E84A4F639EA20F64A52F4158ADA88A4AA839D51810024129BA1791CE461A43EE2AA6B97968374FD32C51CA1710E44A334A9E648CDF7394C7FEA547324E60B1D1A8CC910F18E67DCF808C774410E84CB1456B2CCE4AEE57060237AD411B983F4E96A6F460029AB0F1A844AB618F6CAD50F33E455E23741E4654A144472AAEC2782E5FD4A07E95354DC8CE0359E67D841AE52CD9118DF33341893F14DFC787912C46F8805B0E3CA37E692573B706197299268B1F3160E5A985DB748B192B4C6FB0A276C4D86391E6BEF4503B239E68885FB151AA84830AF4F5CABD00824C91CA3729C42835469E628B55F141AA64E34C789048D2DB2D4D870F9D2A9098F52A69A23155E4B689022C182B38C471286BF4C8E39A2CC1F098D2BCB3747A77C93D0A054B23916E39E84466332CCF1381724342297658ED9381AA1E19A548B798C3D8D30F31827D870BE7634C272BE4EB690919C5F54CA141B6A2A47222C3155AA3992B824D8AE07959F109692326D677676F676B9D706AFB93937D8E7B5B577F588FDBB550855177416E2C23C92DACB8BBEFAB605664BC322DE510E3244F5B373F76152432895686291C1A8D10A2B0D354A65B449A3A80C39B7366CAA57258BA1E21FF6ED47AA156133738AB828A00148922506F595BB0046E599A3B28E08684C36C71C91F336404372591654D23E051822E98C4E780A8ECA4B58DCCA085E0498AB1921D71C59E24F8086966477C096D0CCE799A34A5C0ED0C0926C8B5B88DAFF00BF84EEF0AEA57C44EEB86D95F648FDF62D05C666D6C361B63DEA5B6E46996C922DB1C8D7DA021849DF4959523EB57794A5D208AD9F2C2930D4AB0EF30D34BBE8683FDC5663321F36330BBBEEC36E359E9DC46E5A2ED83778D92B0A6F1AD8FA62C257B0789CC38606FC74D218128ADC325A6108A4DC7E99A2A023714A33EACECB9F962AA454787E61B8BF48F1E7EBF567C9465DE6AD30AC858433926C79D1A04A9ABC5C48582EB583EC2A0815CC00A220B5F1DC2D1950F776182960EC45DB048129DC471624F6A01DD94E230D251112A3D79D120A7D9F7BCB85683D6B66F3410AF7DC3A14D6B11D07A0801862D3509800EF945C283ADB2E1082ED1E5FA4D668484AFDBBB6DD237673ED210C0543BAB2080E0250AA6648BE5FD20C86535C60BAFC2598077E61435315B80091FF00D3ACF4A63139DC3F38E4E220EE4E4CC2599A7A8CFB4D4D604276D046F0F3E563AEB67AF2EAE195387A0289FB08923F85E0F9CF345297C07FBDC078F7DBF7D856A1BFEB6D0FFD9D6D28B0DF6F431C84887A452BFDC2E8BD62499060355A3EC520E1A3C845E4C1E793C9BF8A5AEF9DC53FEEEA8A7B4EE1ABE8BDB3EFFCDB961BF59661D736A966DEB26D88BBDF86F0F301E6BAC89A24B85C9729F44A16C0DFD0CAC7854AEBB71752E1D0FAAE7E7CC8B32E92200B77D6452AA90067BDBAC50431EB85C4062AEB05C50523EB85150DA55561203AA8582F302A70583FA64B828379ADC1C1ECDA50070BEB32018440615D402441C27AB1511A08AC17221FECABB3964305FBEAB24E08A1BEBAB09B0EF3D58B2B7C28AF2EC40CB632B0A1BA3A6DEBF45D92A50EDA54EDA385F2175A7644B0B537A093AADD21BE56DD64B043FA2B512BD59674630C61C561D1BBF100C3D63E4570A7F15F45F29EB3489136F94B8E326ED100E349C2476B1896E772B3B81D8D5764CE55B4F09455D905A7E708B3518CACA829ABF6A0A6736CA3D73BA198E04152546E42748F15D4656594C509EAB55C4B6301F54294C4FB190A6F1016AAE2F974C152C6F2916D57269D95C7F6E9429A32AE4F176D9B8FEA63BE0C5535B7B8D5482CD95EAD9AB65B7B93103CA5D7441703A458C0F50882D241325E59FC90C176C76B313CC860D8DB146D95F40C120161806007B4A7491ACCC6A97C2717A74AEF0926ABA563E3CC54EF2A446C4E6BFE3782675DB31810AC2595759806064B18FE4D8EBBFA9BC6C147DE7CC8F5665D630DBA492C0CC188AE4BAC8AAD8DBEFEA3D6ED0940BB69DF0832601C16C468C47676E5371E9491D77D8DFDDE0883DF3134CAAE4443695C294BE8193108CA98714F34DFF19ADE198E245CFDC29DEC80837E894F6C859C8D18D4646C59537D7C677197BDFBA14B764CD888A77485B08D18A0646C61537D9DB7E3C266158664C7646D5BFBE79625CD780BDD7A5011D1312F3FACE44B14DEDC5E172BA4FC26E164E2DDC768F0CB8B248D877AA10D5A5F179BA173A52D696322088D913B26B11D92216D421EC2411A8844198644066C4834733216E0D9EFCC24ADE80306F08DB12AB8D01A9B2D6BAE0C6B2DF788AD6AAC591F940D3645D48DAA5D71F30D0B6BA5D0AE5042DFAC5D5F898EA7ED2C29A36F56E1C05ED736D9F2B56D9332FAB6156EE1C70FC22289B72259631537078A8FEF5A3E0B33E8F260C155F8B9CFBA1FD77554E8E1905D1B2E788A647D135CC06DA39B0347463122730B82BAE1B027D2A009B2A0552D6AA5044519FE6A97C39C303D6989BFD57685A0E0893CE8D62E4735198429CCCEA6F0B4323C533615C46410960C39752C8296881F38A3A34C1E61B39FF2D7194CFD5503813FD98EA0CB1C62EA328BE821AECE521C455511EEBDFC0266C043279CD324F31F809BA16C6CF18396CC8953585160BBB37BE82DA2AB3C5BE719EA320CEF036697C167325DFB45641696E6E3AB35FE950ED10544A68F2DA5AEA20FB91F7835DDE792177A05043EEC11FB1A3C9619B6B359BDD4489771640844D8579F516F61B80E10587A152D013667B6A70D89DF27B802EE4B638FA102691F0896EDC7673E5825204C0946531FFD4432EC85CF3FFC1F22927473D0B00000 , N'6.1.3-40302')