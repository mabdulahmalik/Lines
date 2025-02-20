<script setup lang="ts">
import {
  IconFilterLines,
  IconSearchOutline,
  IconGoogle,
  IconMicrosoft,
  IconApple,
  IconMail02,
  IconSwitchVertical02,
  IconDotHorizontal,
  IconSlashCircle01,
  IconArrowCircleBrokenDownRight,
} from '@/components/icons/index';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import ButtonGroupRadio from '@/components/form/ButtonGroupRadio.vue';
import Filter from './partials/filter/index.vue';
import { FwbButton, FwbAvatar } from 'flowbite-vue';
import { ref, onMounted, onUnmounted, computed } from 'vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import { useUsersStore } from '@/stores/data/settings/users';
import { useInvitationsStore } from '@/stores/data/settings/users/invitations';
import { UserAvailability,SortEnumType, User } from '@/api/__generated__/graphql';
import { formatRelativeDate } from '@/utils/dateUtils';
import { IconArrowNarrowDown, IconArrowNarrowUp } from '@/components/icons/index';
import DotIndicator from '@/components/status/DotIndicator.vue';
import { useRolesStore } from '@/stores/data/settings/users/roles';
import TableSearchInput from '@/components/form/TableSearchInput.vue';
import ViewUserModal from './partials/modal/ViewUserModal.vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import InviteUsersModal from './partials/modal/InviteUsersModal.vue';

const breadcrumbStore = useBreadcrumbStore();
const usersStore = useUsersStore();
const invitationsStore=useInvitationsStore();
const rolesStore=useRolesStore();

onMounted(() => {
  breadcrumbStore.breadcrumbs = [{ title: 'Settings', to: '/settings/encounters' }, { title: 'Users' }];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

// Search
const modalSearchRef = ref<InstanceType<typeof ModalDrawer>>();
const searchQuery = ref('');
const inputQuery = ref('');
const openSearchModal=()=> {
  modalSearchRef.value?.setModalOpen(true);
}

const performSearch=(query: string)=> {
  searchQuery.value = query;
  modalSearchRef.value?.setModalOpen(false);
}

const searchOptions = computed(() => {
  if (selectedTab.value === 'Active') {
    return usersStore.users
      .filter(user => user.active)
      .flatMap(item => [
        { value: `${item.firstName || ''} ${item.lastName || ''}`, property: 'Name' },
        { value: item.email || '', property: 'Email' },
      ]);
  }

  if (selectedTab.value === 'Deactivated') {
    return usersStore.users
      .filter(user => !user.active)
      .flatMap(item => [
        { value: `${item.firstName || ''} ${item.lastName || ''}`, property: 'Name' },
        { value: item.email || '', property: 'Email' },
      ]);
  }

  if (selectedTab.value === 'Invited') {
    return invitationsStore.invitations.flatMap(invite => {
      const invitedByUser = getInvitedByUser(invite.invitedBy);
      return [
        {
          value: invitedByUser
            ? `${invitedByUser.firstName || ''} ${invitedByUser.lastName || ''}`
            : '',
          property: 'InvitedBy',
        },
        { value: invite.email || '', property: 'Email' },
      ];
    });
  }

  return [
    ...usersStore.users.flatMap(item => [
      { value: `${item.firstName || ''} ${item.lastName || ''}`, property: 'Name' },
      { value: item.email || '', property: 'Email' },
    ]),
    ...invitationsStore.invitations.flatMap(invite => {
      const invitedByUser = getInvitedByUser(invite.invitedBy);
      return [
        {
          value: invitedByUser
            ? `${invitedByUser.firstName || ''} ${invitedByUser.lastName || ''}`
            : '',
          property: 'InvitedBy',
        },
        { value: invite.email || '', property: 'Email' },
      ];
    }),
  ];
});


// Users

// Sort
const sortColumn = ref<string>('loggedInAt'); 
const sortDirection = ref<SortEnumType>(SortEnumType.Asc);

const users = computed(() => {
  return [...usersStore.users]
    .filter(user => {
      if (selectedTab.value === 'Active') return user.active;
      if (selectedTab.value === 'Deactivated') return !user.active;
      if (selectedTab.value === 'Invited') return false;
      return true;
    })
    // Search Query Filtering
    .filter(user => {
      if (!searchQuery.value) return true;
      const userFullName = `${user.firstName} ${user.lastName}`.toLowerCase();
      const userEmail = user.email?.toLowerCase() || '';
      const query = searchQuery.value.toLowerCase();
      return userFullName.includes(query) || userEmail.includes(query);
    })
    // Filter by identity 
    .filter(user => {
      if (allFilters.value.identities.length === 0) return true;
      const identityNames = allFilters.value.identities.map(identity => identity.name.toLowerCase());
      return identityNames.includes(user.identity?.toLowerCase());
    })
    // Filter by role
    .filter(user => {
      if (allFilters.value.role.length === 0) return true;
      return allFilters.value.role.some(filterRole =>
        user.roles?.some(userRole => userRole?.name?.toLowerCase() === filterRole.name.toLowerCase())
      );
    })
      // Filter by last login
      .filter(user => {
      if (allFilters.value.lastLogin.length === 0) return true;
      return allFilters.value.lastLogin.some(filter => {
      if (filter.name === 'Today') {
      return isToday(user.loggedInAt);
      }
      if (filter.name === 'Last 30 Days') {
      return isWithinLast30Days(user.loggedInAt);
      }
      return true;
      });
      })
    .sort((a, b) => compareValues(a, b, sortColumn.value, sortDirection.value));
});

const compareValues = (a: any, b: any, key: string, order: SortEnumType): number => {
  let comparison = 0;
  if (key === 'loggedInAt') {
    comparison = new Date(b.loggedInAt).getTime() - new Date(a.loggedInAt).getTime();
  } else if (key === 'name') {
    comparison = a.firstName.localeCompare(b.firstName) || a.lastName.localeCompare(b.lastName);
  } else if (key === 'email') {
    comparison = a.email.localeCompare(b.email);
  }
  return order === SortEnumType.Asc ? comparison : -comparison;
};
const toggleSort = (column: string) => {
  if (sortColumn.value === column) {
    sortDirection.value = sortDirection.value === SortEnumType.Asc ? SortEnumType.Desc : SortEnumType.Asc;
  } else {
    sortColumn.value = column;
    sortDirection.value = SortEnumType.Asc;
  }
};

// Invitations

// Sort
const inviteSortColumn = ref<string>('invitationSent'); 
const inviteSortDirection = ref<SortEnumType>(SortEnumType.Asc);

const getRoleName = (roleId: string) => {
  const role = rolesStore.roles.find(r => r.id === roleId);
  return role ? role.name : "Unknown Role";
};

const invitations = computed(() => {
  return [...invitationsStore.invitations]
  // Search Query Filtering
  .filter(invite => {
      if (!searchQuery.value) return true; 
      const inviteEmail = invite.email?.toLowerCase() || '';
      const invitedByUser = getInvitedByUser(invite.invitedBy);
      const invitedByName = invitedByUser
        ? `${invitedByUser.firstName} ${invitedByUser.lastName}`.toLowerCase()
        : '';
      const query = searchQuery.value.toLowerCase();
      return inviteEmail.includes(query) || invitedByName.includes(query);
    })
    // sort
  .sort((a, b) => inviteCompareValues(a, b, inviteSortColumn.value, inviteSortDirection.value)
  );
});

const inviteCompareValues = (a: any, b: any, key: string, order: SortEnumType): number => {
  let comparison = 0;
  if (key === 'invitationSent') {
    comparison = new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
  } else if (key === 'invitedBy') {
    const userA = getInvitedByUser(a.invitedBy);
    const userB = getInvitedByUser(b.invitedBy);
    const nameA = userA ? `${userA.firstName} ${userA.lastName}` : "";
    const nameB = userB ? `${userB.firstName} ${userB.lastName}` : "";
    comparison = nameA.localeCompare(nameB);
  } else if (key === 'email') {
    comparison = a.email.localeCompare(b.email);
  }
  return order === SortEnumType.Asc ? comparison : -comparison;
};

const inviteToggleSort = (column: string) => {
  if (inviteSortColumn.value === column) {
    inviteSortDirection.value = inviteSortDirection.value === SortEnumType.Asc ? SortEnumType.Desc : SortEnumType.Asc;
  } else {
    inviteSortColumn.value = column;
    inviteSortDirection.value = SortEnumType.Asc;
  }
};

// Tabs
const selectedTab = ref('Active');
const activeUsersCount = computed(() => usersStore.users.filter(user => user.active).length);
const deactivatedUsersCount = computed(() => usersStore.users.filter(user => !user.active).length);
const invitedUsersCount = computed(() => invitationsStore.invitations.length);

const tabsOptions = computed(() => [
  { value: 'Active', label: `Active (${activeUsersCount.value})` },
  { value: 'Invited', label: `Invited (${invitedUsersCount.value})` },
  { value: 'Deactivated', label: `Deactivated (${deactivatedUsersCount.value})`},
]);

const getInvitedByUser = (invitedById: string ) => {
  return usersStore.users.find(user => user.id === invitedById) || null;
};
const getStatusBadgeClass = (status: string |null | undefined) => {
  switch (status) {
    case UserAvailability.Free:
      return "bg-green-400";
    case UserAvailability.Busy:
      return "bg-yellow-400";
    case UserAvailability.Away:
      return "bg-gray-400";
    case UserAvailability.Offline:
      return "bg-red-400";
    default:
      return "bg-gray-300"; 
  }
};

// Filters

interface Filter {
  name: string;
}
interface allFilters {
  identities: Filter[];
  role: Filter[];
  lastLogin: Filter[];
}

const allFilters = ref<allFilters>({
  identities: [],
  role: [],
  lastLogin: [],
});

const filterDrawerRef = ref<InstanceType<typeof ModalDrawer>>();
const identities = [
  {
    name: "Google",
  },
  {
    name: "Microsoft",
  },
  {
    name: "Apple",
  },
  {
    name: "Email",
  },
];
const role = [
  {
    name: "Administrator",
  },
  {
    name: "Clinician",
  },
  {
    name: "Director",
  },
];
const lastLogin = [
  {
    name: "Today",
  },
  {
    name: "Last 7 Days",
  },
  {
    name: "Last 30 Days",
  },
  {
    name: "Over 30 Days",
  },
];

// Check if a date is today
const isToday = (date: string | Date) => {
  const today = new Date();
  const givenDate = new Date(date);
  return (
    today.getFullYear() === givenDate.getFullYear() &&
    today.getMonth() === givenDate.getMonth() &&
    today.getDate() === givenDate.getDate()
  );
};

// Check if a date is within the last 30 days
const isWithinLast30Days = (date: string | Date) => {
  const givenDate = new Date(date);
  const today = new Date();
  const thirtyDaysAgo = new Date();
  thirtyDaysAgo.setDate(today.getDate() - 30);
  return givenDate >= thirtyDaysAgo;
};
  
// Total filters results count
const hasFilterData = ref(false);
const handleHasFilterData = (val: boolean) => {
  hasFilterData.value = val;
};

const badgeFiltersCount = computed(() => {
  return (
    allFilters.value.identities.length +
    allFilters.value.role.length +
    allFilters.value.lastLogin.length 
  );
});

const handleFiltersData=(filters: allFilters)=> {
  allFilters.value = filters;
}
// Apply filters
const filterRef = ref();
const applyFilters = () => {
  filterRef.value?.submitFilters();
  filterDrawerRef.value?.setModalOpen(false);
};
const clearFilters = () => {
  filterRef.value?.clearFilters();
};

// Edit/View User
const viewUserModalComRef = ref<InstanceType<typeof ViewUserModal>>();
function openUserModal(item: User) {
  usersStore.selectedUser = item;
  viewUserModalComRef.value?.setModalOpen(true);
}

// Cancel Invite
const handleCancelInvite = (id: string) => {
  invitationsStore.cancelUserInvite({inviteId: id});
};

// Resend User Invite
const handleResendUserInvite = (id: string) => {
  invitationsStore.resendUserInvite({inviteId: id});
};

const inviteUsersModalComRef = ref<InstanceType<typeof InviteUsersModal>>();

</script>
<template>
<div>
  <div class="overflow-y-auto max-h-[calc(100vh-120px)] lg:max-h-[calc(100vh-80px)] lg:p-10 p-4">
    <!-- Title & description -->
     <div>
      <div class="font-semibold text-2xl leading-9 text-slate-900">Manage users</div>
      <div class="text-sm font-medium leading-[19.6px] text-slate-900 pt-1">
        You can add users to your company and assign roles.
      </div>
    </div>
    <!--Action bar -->
    <div class="mt-8 flex md:justify-between justify-center flex-wrap gap-4">
      <div class="flex gap-4 items-center">
        <ButtonGroupRadio
        v-model="selectedTab"
        :options="tabsOptions"
        />
        <!-- Filter -->
         <fwb-button
          @click="filterDrawerRef?.setModalOpen(true)"
          color="light"
          pill
          square
          :class="
            [
              'font-semibold border-transparent px-3 ms-1',
              badgeFiltersCount
                ? 'bg-brand-100 text-brand-600 hover:bg-brand-100'
                : 'text-slate-900 bg-transparent',
            ].join(' ')
          "
        >
          <template #prefix>
            <IconFilterLines />
          </template>
          Filter
          <template v-if="badgeFiltersCount" #suffix>
            <div
              class="text-white bg-brand-600 rounded-full text-[12px] leading-[13px] font-medium w-[26px] h-[26px] flex items-center justify-center"
            >
              {{ badgeFiltersCount }}
            </div>
          </template>
        </fwb-button>
      </div>
      <div class="flex gap-4 items-center">
         <!-- Mobile search button -->
           <fwb-button
           @click="openSearchModal"
            color="light"
            class="xl:hidden px-0 border-none focus:ring-0 focus:bg-transparent bg-transparent"
          >
            <template #prefix>
              <IconSearchOutline width="20" height="20" class="text-gray-500 dark:text-gray-400" />
            </template>
            Search
          </fwb-button>
        <!-- Desktop Search -->
        <TableSearchInput
          v-model="inputQuery"
          :options="searchOptions"
          placeholder="Search"
          @search="performSearch"
          class="hidden xl:block w-60"
        />
        <button
          @click="inviteUsersModalComRef?.setModalOpen(true)"
          class="bg-brand-700 py-2 px-3 rounded-full text-white text-sm font-semibold"
        >
          Invite Users
        </button>
      </div>
    </div>
    <!-- Table -->
    <div
      class="my-4 overflow-x-auto py-2 lg:max-w-[calc(100vw-80px)] max-w-[calc(100vw-32px)] "
    >
      <base-table>
        <t-head class="uppercase">
          <t-r>
            <t-h class="w-36 text-nowrap">
                <!-- For invited -->
                <span
                v-if="selectedTab==='Invited'"
                 @click="inviteToggleSort('invitationSent')" 
                  class="inline-flex items-center gap-1 cursor-pointer"
                >
                  <span>Invitation Sent</span>
                  <span
                    class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                  >
                    <template v-if="inviteSortColumn === 'invitationSent'">
                    <IconArrowNarrowUp v-if="inviteSortDirection === SortEnumType.Asc" />
                    <IconArrowNarrowDown v-else />
                    </template>
                    <template v-else>
                   <IconSwitchVertical02 class="opacity-45"/>
                    </template>
                  </span>
                </span>
                <span
                v-else
                @click="toggleSort('loggedInAt')" 
                  class="inline-flex items-center gap-1 cursor-pointer"
                >
                  <span>Last login</span>
                  <span
                    class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                  >
                    <template v-if="sortColumn === 'loggedInAt'">
                    <IconArrowNarrowUp v-if="sortDirection === SortEnumType.Asc" />
                    <IconArrowNarrowDown v-else />
                    </template>
                    <template v-else>
                   <IconSwitchVertical02 class="opacity-45"/>
                    </template>
                  </span>
                </span>
            </t-h>
            <t-h class="min-w-56 text-nowrap">
              <!-- For invited -->
              <span
                  v-if="selectedTab==='Invited'"
                 @click="inviteToggleSort('invitedBy')"
                  class="inline-flex items-center gap-1 cursor-pointer"
                >
                  <span>Invited By</span>
                  <span
                    class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                  >
                    <template v-if="inviteSortColumn === 'invitedBy'">
                    <IconArrowNarrowUp v-if="inviteSortDirection === SortEnumType.Asc" />
                    <IconArrowNarrowDown v-else />
                    </template>
                    <template v-else>
                   <IconSwitchVertical02 class="opacity-45"/>
                    </template>
                  </span>
              </span>
              <span
              v-else
              @click="toggleSort('name')"
              class="inline-flex items-center gap-1 cursor-pointer"
                >
                  <span>Name</span>
                  <span
                    class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                  >
                    <template v-if="sortColumn === 'name'">
                    <IconArrowNarrowUp v-if="sortDirection === SortEnumType.Asc" />
                    <IconArrowNarrowDown v-else />
                    </template>
                    <template v-else>
                   <IconSwitchVertical02 class="opacity-45"/>
                    </template>
                  </span>
              </span>
            </t-h>
            <t-h class="min-w-56 text-nowrap">
              <!-- For invited -->
              <span
              v-if="selectedTab==='Invited'"
               @click="inviteToggleSort('email')"
                class="inline-flex items-center gap-1  cursor-pointer"
                >
                  <span>Email</span>
                  <span
                    class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                  >
                    <template v-if="inviteSortColumn === 'email'">
                    <IconArrowNarrowUp v-if="inviteSortDirection === SortEnumType.Asc" />
                    <IconArrowNarrowDown v-else />
                    </template>
                    <template v-else>
                   <IconSwitchVertical02 class="opacity-45"/>
                    </template>
                  </span>
              </span>
              <span
              v-else
               @click="toggleSort('email')"
                class="inline-flex items-center gap-1 cursor-pointer group"
                >
                  <span>Email</span>
                  <span
                     class="w-[26px] h-[26px] rounded-full flex justify-center items-center hover:bg-gray-100"
                    >
                    <template v-if="sortColumn === 'email'">
                    <IconArrowNarrowUp v-if="sortDirection === SortEnumType.Asc" />
                    <IconArrowNarrowDown v-else />
                    </template>
                    <template v-else>
                   <IconSwitchVertical02 class="opacity-45"/>
                    </template>
                    </span>
                </span>
            </t-h>
            <t-h class="min-w-56 text-nowrap">ROLES</t-h>
            <t-h class="min-w-36 text-nowrap">
              <!-- For invited -->
              <span v-if="selectedTab === 'Invited'">Actions</span>
              <span v-else>Identity</span>
            </t-h>
          </t-r>
        </t-head>
        <t-body>
         <!-- For Invited -->
         <template v-if="selectedTab==='Invited'">
          <t-r  v-for="(inviteUser, index) in invitations" :key="index" class="hover:bg-slate-50">
            <t-d class="text-nowrap">{{ formatRelativeDate(inviteUser.createdAt) }}</t-d>
            <t-d class="text-brand-600 font-semibold">
              <div class="flex gap-2">
                <div class="w-6 h-6 relative">
                  <fwb-avatar  
                  v-if="getInvitedByUser(inviteUser.invitedBy)?.avatar?? ''" 
                  :img="getInvitedByUser(inviteUser.invitedBy)?.avatar?? ''" 
                  rounded 
                  size="xs"
                  ></fwb-avatar>
                  <fwb-avatar v-else rounded size="xs"></fwb-avatar>
                  <span
                    class="h-3 w-3 block border-2 border-white rounded-full absolute -bottom-0.5 -right-1"
                    :class="getStatusBadgeClass(getInvitedByUser(inviteUser.invitedBy)?.status?.status)"
                  ></span>
                </div>               
                {{ getInvitedByUser(inviteUser.invitedBy)?.firstName }} 
                {{ getInvitedByUser(inviteUser.invitedBy)?.lastName }}
              </div>
            </t-d>
            <t-d>{{ inviteUser.email }}</t-d>
            <t-d>
              <div class="flex gap-4 flex-wrap">
                <span
                  v-for="(role, i) in inviteUser.roles"
                  :key="i"
                  class="py-0.5 px-[10px] rounded-full text-xs font-medium"
                   :class="{
                    'bg-green-100 text-green-600 ': getRoleName(role) === 'Administrator',
                    'bg-brand-100 text-brand-700 ': getRoleName(role) === 'Director',
                    'bg-blue-100 text-blue-700 ': getRoleName(role) === 'Clinician',
                    'bg-yellow-100 text-blue-700 ': getRoleName(role) === 'Owner',
                  }" 
                  >  {{ getRoleName(role) }} 
                </span>
              </div>
            </t-d>
            <t-d class="text-slate-900">
              <Dropdown align-to-end placement="left" class="rounded-lg" close-inside>
                <template #trigger>
                  <fwb-button color="light" size="sm" pill square class="border-white">
                    <IconDotHorizontal />
                  </fwb-button>
                </template>
                <DropdownMenu class="!w-36 !min-w-36">
                  <DropdownItem @click="handleCancelInvite(inviteUser.id)">
                    <IconSlashCircle01 width="20" height="20" class="mr-2"/>
                    Revoke
                  </DropdownItem>
                  <DropdownItem @click="handleResendUserInvite(inviteUser.id)">
                    <IconArrowCircleBrokenDownRight width="20" height="20" class="mr-2"/>
                    Resend
                  </DropdownItem>
                </DropdownMenu>
              </Dropdown>
            </t-d>
          </t-r>
          <!-- No results found -->
          <t-r v-if="!invitations || !invitations.length">
            <t-d colspan="6" class="text-center">No results found</t-d>
          </t-r>
         </template>

        <template v-else>
          <t-r  v-for="(user, index) in users" :key="index" class="hover:bg-slate-50">
            <t-d class="text-nowrap">{{ formatRelativeDate(user.loggedInAt) }}</t-d>
            <t-d class="text-brand-600 font-semibold">
              <div @click="openUserModal(user)" class="flex gap-2 cursor-pointer">
                <div class="w-6 h-6 relative">
                  <fwb-avatar v-if="user.avatar" :img="user.avatar" rounded size="xs"></fwb-avatar>
                  <fwb-avatar v-else rounded size="xs"></fwb-avatar>
                  <dot-indicator
                    v-if="user.status?.status"
                    :status="user.status?.status"
                    size="sm"
                    class="h-3 w-3 block border-2 border-white rounded-full absolute -bottom-0.5 -right-1"
                  ></dot-indicator>
                </div>
                {{ user.firstName }} {{ user.lastName }}
              </div>
            </t-d>
            <t-d>{{ user.email }}</t-d>
            <t-d>
              <div class="flex gap-4 flex-wrap">
                <span
                  v-for="(role, i) in user.roles"
                  :key="i"
                  class="py-0.5 px-[10px] rounded-full text-xs font-medium"
                  :class="{
                    'bg-green-100 text-green-600 ': role?.name === 'Administrator',
                    'bg-brand-100 text-brand-700 ': role?.name === 'Director',
                    'bg-blue-100 text-blue-700 ': role?.name === 'Clinician',
                  }"
                  >{{ role?.name }}
                </span>
              </div>
            </t-d>
            <t-d class="text-slate-900">
               <IconApple v-if="user.identity === 'apple'"></IconApple>
               <IconGoogle v-else-if="user.identity === 'google'"> </IconGoogle>
               <IconMicrosoft v-else-if="user.identity === 'microsoft'"></IconMicrosoft>
               <IconMail02 v-else></IconMail02>
            </t-d>
          </t-r>
          <!-- No results found -->
          <t-r v-if="!users || !users.length">
            <t-d colspan="6" class="text-center">No results found</t-d>
          </t-r>
        </template>
        </t-body>
      </base-table>
    </div>
    <!-- Invite users -->
    <InviteUsersModal ref="inviteUsersModalComRef" />
    <!-- Filter drawer -->
    <ModalDrawer ref="filterDrawerRef" max_width="lg" title="Filter" :close_outside="true">
      <template #body>
        <Filter
          ref="filterRef"
          :identities="identities"
          :role="role"
          :lastLogin="lastLogin"
          :selectedFilters="{ ...allFilters }"
          @filtersData="handleFiltersData"
          @hasFilterData="handleHasFilterData"
        ></Filter>
      </template>
      <template #footer>
        <div class="p-4 lg:px-6 flex items-center gap-4 w-full">
          <fwb-button
            v-if="badgeFiltersCount"
            @click="clearFilters"
            color="light"
            pill
            class="w-48 text-nowrap"
          >
            Clear selection
          </fwb-button>
          <fwb-button @click="applyFilters"  class="bg-primary-600 hover:bg-brand-600 w-full" pill>
            {{ hasFilterData ? ' See results' : 'See All' }} 
          </fwb-button>
        </div>
      </template>
    </ModalDrawer>
    <!-- Search Modal -->
     <ModalDrawer
        ref="modalSearchRef"
        max_width="lg"
        title="Search"
        no_footer
        :close_outside="true"
      >
      <template #body>
        <div class="flex flex-col gap-2 p-4">
          <TableSearchInput
            v-model="inputQuery"
            :options="searchOptions"
            placeholder="Search"
            @search="performSearch"
          />
        </div>
      </template>
    </ModalDrawer>
    <!-- Manage User modal -->
    <ViewUserModal
      ref="viewUserModalComRef"
      :user="usersStore.selectedUser"
    />
  </div>
</div>
</template>

