local function incRandoIndex(count)
    memory.u32[0x02164480+0x696] = memory.u32[0x02164480+0x696] + 1
end

local function readAddItemId()
    return memory.u16[0x02164480+0x69A]
end

local function readAddItemCount()
    return memory.u32[0x02164480+0x69C]
end

local function clearAddItem()
    memory.u16[0x02164480+0x69A] = 0xFFFF
    memory.u32[0x02164480+0x69C] = 0
end

local function incGil(count)
    memory.u32[0x02164480-0x1F8] = memory.u32[0x02164480-0x1F8] + count
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
      movsx edx,dword ptr [rax+0x02]
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

local function addItems()
    local id = readAddItemId()
    local count = readAddItemCount()
    if id == 0xFFFE and count > 0 then
        incGil(count)
        clearAddItem()
        incRandoIndex()
    elseif id ~= 0xFFFF and count > 0 then
        addItem(id, count)
        clearAddItem()
        incRandoIndex()
    end
    event.executeAfterMs(100, addItems)
end

local function onExit()
    collectgarbage()
end

print("Rando Open World Archipelago Hook: Applying patch.")

defineAddItem()

event.registerEventAsync("onInitDone", addItems)
event.registerEventAsync("exit", onExit)