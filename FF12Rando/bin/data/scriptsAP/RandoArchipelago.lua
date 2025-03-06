local function incRandoIndex()
    memory.u32[0x02164480+0x696] = memory.u32[0x02164480+0x696] + 1
end

local function getRandoIndex()
    return memory.u32[0x02164480+0x696]
end

local function incGil(count)
    memory.u32[0x02164480-0x1F8] = memory.u32[0x02164480-0x1F8] + count
end

local function readMapID()
    return memory.u32[0x02164480+0x1044]
end

local function readGameState()
    local pointer1 = memory.u32[0x01E5FFE0 + 0x120000]
    return memory.u8[pointer1 + 0x3A]
end

local function readScenarioFlag()
    return memory.u16[0x02164480]
end

local function isOverlayMapOpen()
    return memory.u8[0x02092750] > 0
end

local function defineAddItem()
    argMem = memory.allocExe(0x0100)
    memory.registerSymbol("add_args", argMem)
    --modifyInventory(contentId, count, type, target, logFlags)
    assembly =
    [[
    add_item:
      sub rsp,0x38
      lea rax,[%add_args%]
      movzx ecx,word ptr [rax]
      mov edx,dword ptr [rax+0x02]
      xor r8d,r8d
      lea r9d,[r8+0x01]
      mov [rsp+0x20],r9d
    
      call 0x003008A0
      add rsp,0x38
      ret
    ]]

    memory.assemble(assembly, {"add_item"})
end

local function addItem(id,count)
    argBase = memory.getSymbol("add_args")
    memory.u16[argBase] = id
    memory.s32[argBase+2] = count
    memory.execute("add_item")
end

local function read_comm_file()
    -- Read from %LOCALAPPDATA%/FF12OpenWorldAP/items_received.txt
    local filepath = os.getenv("LOCALAPPDATA") .. "\\FF12OpenWorldAP\\items_received.txt"
    local file = io.open(filepath, "r")
    if not file then return nil end  -- Handle missing file or unable to read at the moment

    local index = getRandoIndex()

    local line
    for i = 0, index do
        line = file:read("*l")  -- Read line by line
        if not line or line == "" then
            file:close()
            return nil  -- Return nil if index is out of bounds
        end
    end
    file:close()

    -- Parse line formatted as <ITEM_ID>|<ITEM_COUNT>
    local id, count = line:match("(%d+)|(%d+)")
    if id and count then
        return {tonumber(id), tonumber(count)}
    end

    return nil  -- Return nil if format is invalid
end

local itemCount = 0
local lastMapID = nil

local function addItems()
    local map_id = readMapID()
    local game_state = readGameState()
    local scenario_flag = readScenarioFlag()

    if map_id == 0 or map_id > 0xFFFF or map_id <= 12 or map_id == 274 or game_state ~= 0 or scenario_flag < 45 then
        event.executeAfterMs(500, addItems)
        return
    end

    if lastMapID ~= map_id then
        lastMapID = map_id
        itemCount = 0
    end

    -- Add items until we reach 50 for this map
    if itemCount >= 50 then
        if not isOverlayMapOpen() then
            message.print(message.convert("Item limit reached for this map. Change maps to get more items."), 0, true)
        end
        event.executeAfterMs(500, addItems)
        return
    end

    local current_item = read_comm_file()
    if current_item == nil then
        event.executeAfterMs(500, addItems)
        return
    end

    local id = current_item[1]
    local count = current_item[2]
    print("Adding item " .. id .. " x" .. count)

    if id == 0xFFFE and count > 0 then
        incGil(count)
    elseif id ~= 0xFFFF and count > 0 then
        addItem(id, count)
        itemCount = itemCount + 1
    end
    incRandoIndex()

    event.executeAfterMs(500, addItems)
end


local function onExit()
    collectgarbage()
end

print("Rando Open World Archipelago Hook: Applying patch.")

defineAddItem()

-- Delete the items_received.txt file on start up
local filepath = os.getenv("LOCALAPPDATA") .. "\\FF12OpenWorldAP\\items_received.txt"
os.remove(filepath)

event.registerEventAsync("onInitDone", addItems)
event.registerEventAsync("exit", onExit)